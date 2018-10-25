using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Cards
{
    static class Program
    {
        public static Dictionary<string, string> Cards = new Dictionary<string, string>();

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName("Cards").Length > 1)
            {
                MessageBox.Show("Приложение уже запущено!");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Macroses.LoadMacros(ref Cards);
            Application.Run(new Macroses());
        }

        public class Macroses : ApplicationContext
        {
            private static NotifyIcon trayIcon;
            private static ContextMenu contextMenu = null;

            public Macroses()
            {
                ReRender();
            }

            public static void ReRender()
            {
                if (trayIcon != null)
                {
                    trayIcon.Icon = null;
                }

                contextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Добавить новый", Create),
                    new MenuItem("Выход", Exit),
                });
                if (Cards.Count > 0)
                {
                    foreach (var check in Cards)
                    {
                        var item = new MenuItem(check.Key, Exit);
                        if (contextMenu.MenuItems.Count == 2)
                            item.BarBreak = true;

                        var copyItem = new MenuItem("Скопировать", Copy);
                        var removeItem = new MenuItem("Удалить", Remove);

                        copyItem.Tag = check.Key;
                        removeItem.Tag = check.Key;

                        item.MenuItems.Add(copyItem);
                        item.MenuItems.Add(removeItem);

                        contextMenu.MenuItems.Add(item);
                    }
                }

                trayIcon = new NotifyIcon()
                {
                    Icon = Resources.TrayIcon,
                    ContextMenu = contextMenu,
                    Visible = true
                };
            }

            #region Functions

            public static void LoadMacros(ref Dictionary<string, string> dictionary)
            {
                if (System.IO.File.Exists("Settings.json"))
                {
                    dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText("Settings.json"));
                }
            }

            public static void SaveMacros()
            {
                try
                {
                    string savedJson = JsonConvert.SerializeObject(Cards);
                    System.IO.File.WriteAllText("Settings.json", savedJson);
                }
                catch (Exception)
                {
                    MessageBox.Show("Запустите приложение от имени Администратора!");
                    return;
                }
            }

            #endregion

            private static void Exit(object sender, EventArgs e)
            {
                trayIcon.Visible = false;

                Application.Exit();
            }

            private static void Create(object sender, EventArgs a)
            {
                AddNew addNew = new AddNew();
                addNew.Show();
            }

            private static void Copy(object sender, EventArgs a)
            {
                if (sender is MenuItem)
                {
                    Clipboard.SetText(Cards[(sender as MenuItem).Tag.ToString()].ToString());
                }
            }

            private static void Remove(object sender, EventArgs a)
            {
                if (sender is MenuItem)
                {
                    Cards.Remove((sender as MenuItem).Tag.ToString());
                    ReRender();
                }

                SaveMacros();
            }
        }
    }
}
