using Starcounter;                                  // Most stuff relating to the database, JSON and communication is in this namespace
using Starcounter.Templates;
using Starcounter.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;

[Launcher_json]                                       // This attribute tells Starcounter that the class corresponds to an object in the JSON-by-example file.
partial class Launcher : Page {

    static String[] RootHtml = File.ReadAllText(@"LauncherTemplate.html").Split(new String[] { "@AppsHtml@" }, StringSplitOptions.RemoveEmptyEntries);

    /// <summary>
    /// Every application in Starcounter works like a console application. They have an .EXE ending. They have a Main() function and
    /// they can do console output. However, they are run inside the scope of a database rather than connecting to it.
    /// </summary>
    static void Main() {

        StarcounterEnvironment.AppName = "Launcher";

        Handle.GET("/", () =>
        {
            Response resp;
            X.GET("/launcher", out resp);

            return resp;
        });

        Handle.MergeResponses((Request req, List<Response> responses, List<String> appNames) =>
        {
            StringBuilder sb = new StringBuilder();

            switch (Session.InitialRequest.PreferredMimeType)
            {
                case MimeType.Text_Html:
                {
                    // Going through each response and appending it.
                    for (Int32 i = 0; i < responses.Count; i++)
                    {
                        sb.Append("<template bind=\"{{" + appNames[i] + "}}\">\n");
                        sb.Append(responses[i].GetContentString(MimeType.Text_Html));
                        sb.Append("</template>\n");
                    }

                    break;
                }

                case MimeType.Application_Json:
                {
                    sb.Append("{");
                    Int32 n = responses.Count;
                    for (Int32 i = 0; i < n; i++)
                    {
                        sb.Append("\"" + appNames[i] + "\":");
                        sb.Append(responses[i].GetContentString(MimeType.Application_Json));
                        if (i < n - 1)
                            sb.Append(",");
                    }
                    sb.Append("}");

                    break;
                }

                default:
                    throw new ArgumentException("Request is of unsuitable MIME type to merge responses!");
            }

            Response mergedResp = new Response()
            {
                Body = sb.ToString()
            };

            return mergedResp;
        });

        Handle.GET("/launcher", (Request req) =>
        {
            /*
            Launcher m = new Launcher()
            {                                       // This is the root view model for our application. A view model is a JSON object/tree.
                Html = "/launcher.html",              // This is just a field we added to allow the client to know what Html to load. No magic.
            };
            m.Session = new Session();              // Save the JSON on the server to be accessed using GET/PATCH to allow it to be used as a view model in web components.

            var j = (LauncherWorkspace)X.GET("/whoareyou_combined"); // We are cheating as multiple handlers are not allowed and there is no merger code to merge them together anyways.
            m.workspace = j;
            */

            Response resp;
            X.GET("/whoareyou", out resp);

            if (Session.InitialRequest.PreferredMimeType == MimeType.Text_Html)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(RootHtml[0]);
                sb.Append(resp.Body);
                sb.Append(RootHtml[1]);
                resp.Body = sb.ToString();
            }

            if (Session.Current == null)
                Session.Current = new Session();

            return resp;                               // Return the JSON or the HTML depending on the type asked for. See Page.json on how Starcounter knowns what to return.
        });

        Handle.GET("/whoareyou", () =>
        {
            var eTemplate = new TObject();
            eTemplate.Add<TString>("firstName$");

            dynamic e = new Json();
            e.Template = eTemplate;
            e.firstName = "Enecto!!";
            e.Html = "<article><input value=\"{{firstName$}}\"></article>";

            return e;
        });

        Handle.GET("/whoareyou", () =>
        {
            var sTemplate = new TObject();
            sTemplate.Add<TString>("title");
            sTemplate.Add<TTrigger>("call$");

            dynamic s = new Json();
            s.Template = sTemplate;
            s.title = "Skype!!!!";
            s.Html = "<div><button onclick=\"this.model.call$ = null\" value=\"{{call$}}\">{{title}}</button></div>";

            return s;
        });

        /*
        Handle.GET("/whoareyou_combined", () =>
        {
            var e = (Json)X.GET("/whoareyou1");
            var s = (Json)X.GET("/whoareyou2");

            var page = new LauncherWorkspace();
            page.appThumbnails.Add(e);
            page.appThumbnails.Add(s);

            return page;
        });
       */
    }

}





