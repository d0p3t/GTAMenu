using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using GTAMenu;

namespace GTAMenuExample
{
    public class MenuExample : BaseScript
    {
        private NativeMenuManager _nativeMenuManager;
        private NativeMenu mainMenu;

        private bool ketchup = false;
        private string dish = "Banana";

        private void AddMenuKetchup(NativeMenu menu)
        {
            var ketchupItem = new NativeMenuCheckboxItem("Add ketchup?", "Do you wish to add ketchup?", ketchup);
            menu.MenuItems.Add(ketchupItem);
            ketchupItem.Check += (sender, e) =>
            {
                if (e.MenuItem == ketchupItem)
                {
                    ketchup = e.Checked;
                    Screen.ShowNotification("~r~Ketchup status: ~b~" + ketchup);
                }
            };
        }

        private void AddMenuFoods(NativeMenu menu)
        {
            object[] foods =
            {
                "Banana",
                "Apple",
                "Pizza",
                "Quartilicious"
            };
            var foodsItem = new NativeMenuListItem("Food", "Description", foods, 0);
            menu.MenuItems.Add(foodsItem);
            foodsItem.ChangedIndex += (sender, e) =>
            {
                dish = foodsItem.CurrentValue;
                Screen.ShowNotification("Preparing ~b~" + dish + "~w~...");
            };
        }

        private void AddMenuCook(NativeMenu menu)
        {
            var newItem = new NativeMenuItemBase("Cook!", "Cook the dish with the appropiate ingredients and ketchup.");
            menu.MenuItems.Add(newItem);
            menu.ItemSelected += (sender, e) =>
            {
                if (e.MenuItem == newItem)
                {
                    var output = ketchup ? "You have ordered ~b~{0}~w~ ~r~with~w~ ketchup." : "You have ordered ~b~{0}~w~ ~r~without~w~ ketchup.";
                    Screen.ShowSubtitle(String.Format(output, dish));
                }
            };
        }

        private void AddMenuAnotherMenu(NativeMenu menu)
        {
            var subMenu = _nativeMenuManager.AddSubMenu("Another Menu", "Description of Another Menu", "Another Menu", "Description of Another Menu", menu);
            for (int i = 0; i < 20; i++)
                subMenu.MenuItems.Add(new NativeMenuItemBase("PageFiller", "Sample description that takes more than one line. Moreso, it takes way more than two lines since it's so long. Wow, check out this length!"));
        }

        public MenuExample()
        {
            _nativeMenuManager = new NativeMenuManager();
            mainMenu = new NativeMenu("GTAMenu", "~b~GTAMENU SHOWCASE", MenuBannerType.InteractionMenu);
            _nativeMenuManager.AddMenu(mainMenu);
            AddMenuKetchup(mainMenu);
            AddMenuFoods(mainMenu);
            AddMenuCook(mainMenu);
            AddMenuAnotherMenu(mainMenu);

            Tick += OnTick;
        }

        private async Task OnTick()
        {
            try
            {
                _nativeMenuManager.ProcessMenus();

                if (Game.IsControlJustReleased(0, Control.SelectCharacterMichael) && !_nativeMenuManager.IsAnyMenuOpen())
                {
                    mainMenu.Visible = !mainMenu.Visible;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.Message} : Exception thrown on MenuExample:OnTick()");
            }

            await Task.FromResult(0);
        }
    }
}
