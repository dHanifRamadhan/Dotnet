using Project01.Entities;

namespace Project01.Helpers {
    public class ModifyFile {
        public void AddEnumPage(Page item) {
            var enumContent = File.ReadAllText(PathFile.ENUM_PAGES);
            if (!enumContent.Contains(item.PageName)) {
                var insert = enumContent.LastIndexOf("    }");
                var newValue = $"        {item.PageName},\n";
                var update = enumContent.Insert(insert, newValue);
                File.WriteAllText(PathFile.ENUM_PAGES, update);
            }
        }

        public void UpdateEnumPage(Page item, string oldName) {
            var enumContent = File.ReadAllText(PathFile.ENUM_PAGES);
            if (enumContent.Contains(oldName)) {
                var update = enumContent.Replace(oldName, item.PageName);
                File.WriteAllText(PathFile.ENUM_PAGES, update);
            }
        }

        public void DeleteEnumPage(Page item) {
            var enumContent = File.ReadAllText(PathFile.ENUM_PAGES);
            if (enumContent.Contains(item.PageName)) {
                var update = enumContent.Replace(item.PageName+",", "");
                File.WriteAllText(PathFile.ENUM_PAGES, update);
            }
        }
    }
}