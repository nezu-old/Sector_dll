using Sector_dll.sdk;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static Sector_dll.cheat.Drawing;

namespace sectorsedge.cheat
{
    class Menu
    {

        private const int MenuItemH = 15;
        private const int MenuIndent = 8;
        private static readonly Color inactiveColor = new Color(200, 200, 200);
        private static readonly Color activeColor = new Color(30, 160, 30);
        private static readonly Color backgroundColor = new Color(0, 0, 0, 230);

        public delegate T GetValueDelegate<T>();
        public delegate void SetValueDelegate<T>(T value);

        public static bool test = false;

        public static int menuX = 300;
        public static int menuY = 100;

        private static List<IMenuItem> menuItems = new List<IMenuItem>();

        static Menu()
        {
            menuItems.Add(new Togle("test1", () => test, v => test = v));

            Category aim = new Category("aim");
            aim.AddChild(new Togle("test2", () => true, v => test = v));
            aim.AddChild(new Togle("test3", () => true, v => test = v));
            menuItems.Add(aim);

            Category esp = new Category("esp");
            esp.AddChild(new Togle("test4", () => test, v => test = v));
            esp.AddChild(new Togle("test5", () => true, v => test = v));
            esp.expanded = true;
            menuItems.Add(esp);

            Category misc = new Category("misc");
            misc.AddChild(new Togle("test6", () => test, v => test = v));
            misc.AddChild(new Togle("test7", () => test, v => test = v));
            misc.expanded = true;
            menuItems.Add(misc);

            menuItems.Add(new Togle("test8", () => true, v => test = v));
            menuItems.Add(new Togle("test9", () => true, v => test = v));
        }

        public static void Draw()
        {
            DrawRectFilled(menuX, menuY, 100, 300, backgroundColor);

            int y = menuY + 10;
            int x = menuX + 10;
            foreach (IMenuItem item in menuItems)
            {
                item.Draw(x, y, out int yo);
                y += yo;
            }

            

        }

        class Togle : IMenuItem
        {

            public string name;

            private GetValueDelegate<bool> getValue;

            private SetValueDelegate<bool> setValue;

            public Togle(string name, GetValueDelegate<bool> getValue, SetValueDelegate<bool> setValue)
            {
                this.name = name;
                this.getValue = getValue;
                this.setValue = setValue;
            }

            public int Draw(int startX, int startY, out int yOffset)
            {
                DrawText(name, startX, startY, getValue() ? activeColor : inactiveColor);

                yOffset = MenuItemH;
                return 1;
            }

        }

        class Category : IMenuItem
        {
            private readonly List<IMenuItem> _children = new List<IMenuItem>();

            public bool expanded = false;

            public string name;

            public Category(string name)
            {
                this.name = name;
            }

            public int Draw(int startX, int startY, out int yOffset)
            {
                int drawn = 1; //Category header

                DrawText(name, startX, startY);

                int x = startX + MenuIndent; // add title x offset
                int y = startY + MenuItemH; //indenyt children

                if (expanded)
                {
                    foreach (IMenuItem item in _children)
                    {
                        drawn += item.Draw(x, y, out int yo);
                        y += yo;
                    }
                }

                yOffset = y - startY;
                return drawn;
            }

            public void AddChild(IMenuItem item) => _children.Add(item);

            public IMenuItem[] GetChildren() => _children.ToArray();

        }

        interface IMenuItem
        {
            int Draw(int startX, int startY, out int yOffset);



        }

    }
}
