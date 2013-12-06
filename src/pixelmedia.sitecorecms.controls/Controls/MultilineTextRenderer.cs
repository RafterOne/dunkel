using System;
using System.Web.UI;
using Sitecore.Web.UI.WebControls;

namespace PixelMEDIA.SitecoreCMS.Controls.Controls
{
    /// <summary>
    /// Control to render mult-line text with option to stop Sitecore from replacing line feeds with
    /// break tags. Needed when outputting  javascript.
    /// </summary>
   public class MultilineTextRenderer : FieldRenderer
    {
        private bool _addScriptTags = false;
        private bool _disableBreakTags = false;

        public bool AddScriptTags
        {
            get { return _addScriptTags; }
            set { _addScriptTags = value; }
        }

        public bool DisableBreakTags
        {
            get { return _disableBreakTags; }
            set { _disableBreakTags = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {
            if (Item == null)
            {
                Item = this.GetItem();
            }
            if (this.Item != null)
            {
                var dataField = this.Item.Fields[this.FieldName];
                if (null == dataField) { return; }
                var content = dataField.Value;
                if (!String.IsNullOrEmpty(content))
                {
                   if (AddScriptTags)
                   {
                       output.RenderBeginTag(HtmlTextWriterTag.Script);
                   }
                    if (DisableBreakTags)
                    {
                        output.Write(dataField.Value);
                    }
                    else
                    {
                        base.DoRender(output);
                    }
                    if (AddScriptTags)
                    {
                        output.RenderEndTag();
                    }
                }
            }
        }
    }
}
