using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChroMapperAutoBookmarker
{
    static class PromptManager
    {
        private const int STARTING_BEAT_STATE = 1;
        private const int INTERVAL_STATE = 2;
        private const int AMOUNT_STATE = 3;
        private const int NAME_STATE = 4;
        private const int GENERATE_STATE = 5;
        private static int state = 0;

        private static float StartBeat { get; set; }
        private static float Interval { get; set; }
        private static int Amount { get; set; }
        private static string Name { get; set; }

        private static void NextPrompt()
        {
            state++;
            CoroutineStarter.Start(ShowPrompt());
        }

        private static IEnumerator ShowPrompt()
        {
            yield return null; //This is to get around a bug with the input boxes where you need to wait a frame before the next one or you don't get a response.
            switch (state)
            {
                case STARTING_BEAT_STATE:
                    PersistentUI.Instance.ShowInputBox("Please input the starting beat", (startBeatS) =>
                    {
                        if (startBeatS == null)
                        {
                            Utils.SetActionMapsEnabled(true);
                            return;
                        }
                        if (!float.TryParse(startBeatS, out float startBeat))
                        {
                            ShowInvalid();
                            return;
                        }
                        StartBeat = startBeat;
                        NextPrompt();
                    }, "4");
                    break;
                case INTERVAL_STATE:
                    PersistentUI.Instance.ShowInputBox("Please input how many beats between each bookmark", (intervalS) =>
                    {
                        if (intervalS == null)
                        {
                            Utils.SetActionMapsEnabled(true);
                            return;
                        }
                        if (!float.TryParse(intervalS, out float interval))
                        {
                            ShowInvalid();
                            return;
                        }
                        Interval = interval;
                        NextPrompt();
                    }, "16");
                    break;
                case AMOUNT_STATE:
                    PersistentUI.Instance.ShowInputBox("How many would you like to generate? (Enter -1 to fill the whole song)", (amountS) =>
                    {
                        if (amountS == null)
                        {
                            Utils.SetActionMapsEnabled(true);
                            return;
                        }
                        if (!int.TryParse(amountS, out int amount))
                        {
                            ShowInvalid();
                            return;
                        }
                        Amount = amount;
                        NextPrompt();
                    }, "-1");
                    break;
                case NAME_STATE:
                    PersistentUI.Instance.ShowInputBox("What would you like to call these bookmarks?", (name) =>
                    {
                        if (name == null)
                        {
                            Utils.SetActionMapsEnabled(true);
                            return;
                        }
                        Name = name;
                        NextPrompt();
                    });
                    break;
                case GENERATE_STATE:
                    BookmarkManager bookmarkManager = Resources.FindObjectsOfTypeAll<BookmarkManager>().First();
                    AudioTimeSyncController atsc = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().First();
                    float lastBeat = atsc.GetBeatFromSeconds(atsc.songAudioSource.clip.length);

                    float currentBeat = StartBeat;
                    int count = 1;
                    int amountLeft = Amount;
                    while (amountLeft == -1 && currentBeat <= lastBeat || amountLeft > 0)
                    {
                        BeatmapBookmark newBookmark = new BeatmapBookmark(currentBeat, string.IsNullOrWhiteSpace(Name) ? $"{count}" : $"{Name} {count}");
                        BookmarkContainer container = MonoBehaviour.Instantiate(bookmarkManager.GetField<GameObject>("bookmarkContainerPrefab"), bookmarkManager.transform).GetComponent<BookmarkContainer>();
                        container.name = newBookmark._name;
                        container.Init(bookmarkManager, newBookmark);
                        container.RefreshPosition(bookmarkManager.GetField<RectTransform>("timelineCanvas").sizeDelta.x + bookmarkManager.GetField<float>("CANVAS_WIDTH_OFFSET"));
                        bookmarkManager.GetField<List<BookmarkContainer>>("bookmarkContainers").Add(container);
                        currentBeat += Interval;
                        if (amountLeft > 0)
                            amountLeft--;
                        count++;
                    }
                    BeatSaberSongContainer.Instance.map._bookmarks = bookmarkManager.GetField<List<BookmarkContainer>>("bookmarkContainers").Select(x => x.data).ToList();
                    Utils.SetActionMapsEnabled(true);
                    break;
            }
        }

        public static void OnClick()
        {
            Utils.SetActionMapsEnabled(false);
            state = 0;
            NextPrompt();
        }

        private static void ShowInvalid()
        {
            PersistentUI.Instance.ShowDialogBox("Invalid input", (response) => {
                Utils.SetActionMapsEnabled(true);
            }, PersistentUI.DialogBoxPresetType.Ok);
        }
    }
}
