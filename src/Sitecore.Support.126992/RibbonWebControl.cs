using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceEditor.Utils;
using Sitecore.SecurityModel;
using Sitecore.Shell.Web;
using Sitecore.Sites;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.Bundling;
using Sitecore.Web.UI;
using Sitecore.Web.UI.HtmlControls;
using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.UI;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.PageExtender
{
    public class RibbonWebControl : Sitecore.ExperienceEditor.Speak.Ribbon.PageExtender.RibbonWebControl
    {
        protected override void DoRender(HtmlTextWriter output)
        {
            Sitecore.Sites.SiteContext site = Sitecore.Configuration.Factory.GetSite("shell");

            // Sitecore.Support.126992
          /*  using (new Sitecore.Sites.SiteContextSwitcher(site))
            {
                
                Sitecore.Shell.Web.ShellPage.IsLoggedIn(false);
            }*/
            string mode = this.Mode;
            bool webEdit = mode == "edit";
            bool debug = mode == "debug";
            List<string> list = ScriptResources.GenerateBaseContentEditingScriptsList(webEdit, debug);
            foreach (string current in list)
            {
                output.Write(Sitecore.Web.HtmlUtil.GetClientScriptIncludeHtml(current));
            }
            if (WebUtility.IsSublayoutInsertingMode)
            {
                return;
            }
            this.RenderResources(output);
            output.Write("<link href=\"{0}\" rel=\"stylesheet\" />", Sitecore.Configuration.Settings.WebEdit.ContentEditorStylesheet);
            WebUtility.RenderLoadingIndicator(output);
            output.Write("<link href=\"{0}\" rel=\"stylesheet\" />", Sitecore.ExperienceEditor.Settings.WebEdit.ExperienceEditorStylesheet);
            string value = string.Format("<iframe id=\"scWebEditRibbon\" src=\"{0}\" class=\"scSpeakWebEditRibbon scWebEditRibbon scFixedRibbon\" frameborder=\"0\" marginwidth=\"0\" marginheight=\"0\" height=\"50px\" width=\"100%\"></iframe>", this.Url);
            output.Write(value);
            string device = WebUtility.GetDevice(new Sitecore.Text.UrlString(this.Url));
            WebUtility.RenderLayout(Sitecore.Context.Item, output, Sitecore.Context.Site.Name, device);
            if (Sitecore.Context.Site.DisplayMode == Sitecore.Sites.DisplayMode.Edit)
            {
                List<string> capabilities = RibbonWebControl.GetCapabilities();
                output.Write("<input type=\"hidden\" id=\"scCapabilities\" value=\"" + string.Join("|", capabilities.ToArray()) + "\" />");
            }
            output.Write("<input type=\"hidden\" id=\"scLayoutDefinition\" name=\"scLayoutDefinition\" />");
            output.Write(AntiForgery.GetHtml());
        }

        private static List<string> GetCapabilities()
        {
            List<string> list = new List<string>();
            if (Sitecore.SecurityModel.Policy.IsAllowed("Page Editor/Can Design") && Sitecore.Web.UI.HtmlControls.Registry.GetString("/Current_User/Page Editor/Capability/design") != Sitecore.ExperienceEditor.Constants.Registry.CheckboxUnTickedRegistryValue)
            {
                list.Add("design");
            }
            if (Sitecore.SecurityModel.Policy.IsAllowed("Page Editor/Can Edit") && Sitecore.Web.UI.HtmlControls.Registry.GetString("/Current_User/Page Editor/Capability/edit") != Sitecore.ExperienceEditor.Constants.Registry.CheckboxUnTickedRegistryValue)
            {
                list.Add("edit");
            }
            if (Sitecore.SecurityModel.Policy.IsAllowed("Page Editor/Extended features/Personalization"))
            {
                list.Add("personalization");
            }
            if (Sitecore.SecurityModel.Policy.IsAllowed("Page Editor/Extended features/Testing"))
            {
                list.Add("testing");
            }
            return list;
        }
    }
}