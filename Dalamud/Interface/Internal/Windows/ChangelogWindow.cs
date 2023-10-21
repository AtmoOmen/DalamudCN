using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;

using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using ImGuiNET;
using ImGuiScene;
using Serilog;

namespace Dalamud.Interface.Internal.Windows;

/// <summary>
/// For major updates, an in-game Changelog window.
/// </summary>
internal sealed class ChangelogWindow : Window, IDisposable
{
    /// <summary>
    /// Whether the latest update warrants a changelog window.
    /// </summary>
    public const string WarrantsChangelogForMajorMinor = "7.5.1.3";

    private const string ChangeLog =
        @"• 重置了代理设置,取消了F**KGFW设置,如有必要请重新设置代理,重启游戏后生效
    
如果你有任何问题或者需要帮助，请查看FAQ，或者到QQ频道求助!";

    private const string UpdatePluginsInfo =
            @"• 由于此更新，您的所有插件可能会被禁用。 这是正常的。
• 打开插件安装程序，然后单击“更新插件”。更新的插件应该更新然后重新启用自己。
    => 请记住，并非所有插件都已针对新版本进行了更新。
    => 如果某些插件在“已安装插件”选项卡中显示为红色叉号，则它们可能尚不可用。";

    private readonly string assemblyVersion = Util.AssemblyVersion;

    private readonly IDalamudTextureWrap logoTexture;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangelogWindow"/> class.
    /// </summary>
    public ChangelogWindow()
        : base("有啥新功能？？", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoResize)
    {
        this.Namespace = "DalamudChangelogWindow";

        this.Size = new Vector2(885, 463);
        this.SizeCondition = ImGuiCond.Appearing;

        this.logoTexture = Service<Branding>.Get().Logo;
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        ImGui.Text($"卫月框架更新到了版本 D{this.assemblyVersion}。");

        ImGuiHelpers.ScaledDummy(10);

        ImGui.Text("包含了以下更新:");

        ImGui.SameLine();
        ImGuiHelpers.ScaledDummy(0);
        var imgCursor = ImGui.GetCursorPos();

        ImGui.TextWrapped(ChangeLog);

        ImGuiHelpers.ScaledDummy(5);

        ImGui.TextColored(ImGuiColors.DalamudRed, " !!! 注意 !!!");

        ImGui.TextWrapped(UpdatePluginsInfo);

        ImGuiHelpers.ScaledDummy(10);

        // ImGui.Text("感谢使用我们的工具！");

        // ImGuiHelpers.ScaledDummy(10);

        ImGui.PushFont(UiBuilder.IconFont);

        if (ImGui.Button(FontAwesomeIcon.Download.ToIconString()))
        {
            Service<DalamudInterface>.Get().OpenPluginInstaller();
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.PopFont();
            ImGui.SetTooltip("打开插件安装器");
            ImGui.PushFont(UiBuilder.IconFont);
        }

        ImGui.SameLine();

        if (ImGui.Button(FontAwesomeIcon.LaughBeam.ToIconString()))
        {
            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "https://discord.gg/3NMcUV5",
                    UseShellExecute = true,
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not open discord url");
            }
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.PopFont();
            ImGui.SetTooltip("加入我们的 Discord 服务器（国际服）");
            ImGui.PushFont(UiBuilder.IconFont);
        }

        ImGui.SameLine();

        if (ImGui.Button(FontAwesomeIcon.LaughSquint.ToIconString()))
        {
            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "https://qun.qq.com/qqweb/qunpro/share?_wv=3&_wwv=128&appChannel=share&inviteCode=CZtWN&businessType=9&from=181074&biz=ka&shareSource=5",
                    UseShellExecute = true,
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not open QQ url");
            }
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.PopFont();
            ImGui.SetTooltip("加入我们的 QQ 频道");
            ImGui.PushFont(UiBuilder.IconFont);
        }

        ImGui.SameLine();

        if (ImGui.Button(FontAwesomeIcon.Globe.ToIconString()))
        {
            Util.OpenLink("https://ottercorp.github.io/faq/");
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.PopFont();
            ImGui.SetTooltip("查看 FAQ");
            ImGui.PushFont(UiBuilder.IconFont);
        }

        ImGui.SameLine();

        if (ImGui.Button(FontAwesomeIcon.Heart.ToIconString()))
        {
            Util.OpenLink("https://ottercorp.github.io/faq/support");
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.PopFont();
            ImGui.SetTooltip("支持我们");
            ImGui.PushFont(UiBuilder.IconFont);
        }

        ImGui.PopFont();

        ImGui.SameLine();
        ImGuiHelpers.ScaledDummy(20, 0);
        ImGui.SameLine();

        if (ImGui.Button("关闭"))
        {
            this.IsOpen = false;
        }

        imgCursor.X += 750;
        imgCursor.Y -= 30;
        ImGui.SetCursorPos(imgCursor);

        ImGui.Image(this.logoTexture.ImGuiHandle, new Vector2(100));
    }

    /// <summary>
    /// Dispose this window.
    /// </summary>
    public void Dispose()
    {
        this.logoTexture.Dispose();
    }
}
