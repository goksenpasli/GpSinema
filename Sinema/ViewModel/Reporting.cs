using NGS.Templater;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace Sinema.ViewModel
{
    public static class Reporting
    {
        public static void CreateReport(this IEnumerable dbset, string xlsxfilepath)
        {
            using Stream stream = File.Open(xlsxfilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var dosyaismi = Path.GetTempPath() + Guid.NewGuid() + ".xlsx";
            using (FileStream output = new(dosyaismi, FileMode.Create))
            {
                using ITemplateDocument document = Configuration.Factory.Open(stream, output, "xlsx");
                document.Process(new { Context = dbset });
            }
            Process.Start(dosyaismi);
        }
    }
}