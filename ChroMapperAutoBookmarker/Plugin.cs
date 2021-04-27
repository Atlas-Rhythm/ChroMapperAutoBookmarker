namespace ChroMapperAutoBookmarker
{
    [Plugin("Auto Bookmarker")]
    public class Plugin
    {
        [Init]
        private void Init()
        {
            ExtensionButtons.AddButton(Utils.LoadSprite("ChroMapperAutoBookmarker.Icon.png"), "Generate Bookmarks", PromptManager.OnClick);
        }

        [Exit]
        private void Exit() { }
    }
}
