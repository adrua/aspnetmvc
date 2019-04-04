using System.Web;
using System.Web.Optimization;

namespace IconexInventarios
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.number.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                        "~/Scripts/free-jqGrid/jquery.jqgrid.min.js",
                        "~/Scripts/free-jqGrid/i18n/grid.locale-es.js",
                        "~/Scripts/free-jqGrid/plugins/ui.multiselect.js",
                        "~/Scripts/free-jqGrid/plugins/jquery.tablednd.js",
                        "~/Scripts/free-jqGrid/plugins/jquery.searchFilter.js",
                        "~/Scripts/free-jqGrid/plugins/jquery.contextmenu.js",
                        "~/Scripts/free-jqGrid/plugins/grid.extensions.arkeos.js"));

            bundles.Add(new ScriptBundle("~/Content/jqgrid/css").Include(
            "~/Content/ui.jqgrid.min.css",
            "~/Scripts/free-jqGrid/plugins/css/ui.multiselect.css",
            "~/Scripts/free-jqGrid/plugins/css/searchfilter.css"));


            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            "~/Content/themes/base/core.css",
            "~/Content/themes/base/resizable.css",
            "~/Content/themes/base/selectable.css",
            "~/Content/themes/base/accordion.css",
            "~/Content/themes/base/autocomplete.css",
            "~/Content/themes/base/button.css",
            "~/Content/themes/base/dialog.css",
            "~/Content/themes/base/slider.css",
            "~/Content/themes/base/tabs.css",
            "~/Content/themes/base/datepicker.css",
            "~/Content/themes/base/progressbar.css",
            "~/Content/themes/base/theme.css"));


        }
    }
}
