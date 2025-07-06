using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Resources;

[HtmlTargetElement("dt", Attributes = "display-for")]
[HtmlTargetElement("th", Attributes = "display-for")]

public class MyTagHelper : TagHelper
{
    [HtmlAttributeName("display-for")]
    public ModelExpression DisplayFor { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var metadata = DisplayFor.Metadata;
        var displayName = metadata.DisplayName ?? DisplayFor.Name;
        output.Content.SetContent(displayName);
    }
}

/// <summary>
/// This helper does not work well with nested models (InsuredPersonDetailDto / List<PersonInsuranceDto> PersonInsurances).
/// Remained here to demonstrate the issue and way of handling nested models.
/// </summary>
/*[HtmlTargetElement("th", Attributes = "display-for-model")]
public class MyInnerListTagHelper : TagHelper
{
    [HtmlAttributeName("display-for-model")]
    public string DisplayForModel { get; set; }

    [HtmlAttributeName("model-type")]
    public Type ModelType { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ModelType == null || string.IsNullOrWhiteSpace(DisplayForModel))
        {
            output.Content.SetContent(DisplayForModel);
            return;
        }

        var prop = ModelType.GetProperty(DisplayForModel);
        if (prop == null)
        {
            output.Content.SetContent(DisplayForModel); // fallback na název vlastnosti
            return;
        }

        var displayAttr = prop.GetCustomAttribute<DisplayAttribute>();
        string label = DisplayForModel;

        if (displayAttr != null)
        {
            Debug.WriteLine($"ATTR found for {DisplayForModel}: name={displayAttr.Name}, resourceType={displayAttr.ResourceType}"); // debug

            if (displayAttr.ResourceType != null && !string.IsNullOrEmpty(displayAttr.Name))
            {
                var resourceProp = displayAttr.ResourceType
                    .GetProperty(displayAttr.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                label = resourceProp?.GetValue(null)?.ToString() ?? displayAttr.Name;
            }
            else
            {
                label = displayAttr.Name ?? DisplayForModel;
            }
        }
        else
        {
            Debug.WriteLine($"No DisplayAttribute found for {DisplayForModel}"); // debug
        }

            output.Content.SetContent(label);
    }
}
*/
