using Sitecore.ExperienceEditor.Pipelines.GetExperienceEditorRibbon;
using Sitecore.ExperienceEditor.Speak.Ribbon.PageExtender;
using Sitecore.Publishing;
using System;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Pipelines.GetExperienceEditorRibbon
{
    public class AddWebEditRibbon : WebEditStateProcessor
    {
        public override void AddControl(GetExperienceEditorRibbonArgs args)
        {
            if (WebEditStateProcessor.IsWebEditContent())
            {
                return;
            }
            string text = string.Empty;
            if (WebEditStateProcessor.IsWebEditState())
            {
                text = Sitecore.ExperienceEditor.Speak.Ribbon.Constants.Ribbons["edit"];
                Sitecore.Publishing.PreviewManager.RestoreUser();
            }
            else if (WebEditStateProcessor.IsPreviewState())
            {
                text = Sitecore.ExperienceEditor.Speak.Ribbon.Constants.Ribbons["preview"];
            }
            else if (WebEditStateProcessor.IsDebugState())
            {
                text = Sitecore.ExperienceEditor.Speak.Ribbon.Constants.Ribbons["debug"];
            }
            if (text == string.Empty)
            {
                return;
            }
            // Sitecore.Support.126992
            args.Control = new Sitecore.Support.ExperienceEditor.Speak.Ribbon.PageExtender.RibbonWebControl
            {
                State = text
            };
        }
    }
}
