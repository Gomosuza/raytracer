namespace Raytracer.Configuration
{
    public class Settings
    {
        public SceneSettings Scene { get; set; } = new SceneSettings();

        public ComputeSettings Compute { get; set; } = new ComputeSettings();

        public VideoSettings Video { get; set; } = new VideoSettings();

        public KeyboardSettings Keybindings { get; set; } = new KeyboardSettings();

        public MouseSettings Mouse { get; set; } = new MouseSettings();
    }
}
