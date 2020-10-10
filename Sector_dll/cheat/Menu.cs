using Sector_dll.sdk;
using Sector_dll.util;
using sectorsedge.cheat.Drawing.Fonts;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using static Sector_dll.cheat.Drawing;
using static Sector_dll.cheat.Config;
using System;
using System.Linq;

namespace sectorsedge.cheat
{
    class Menu
    {

        private const Keys menuKey = Keys.Insert;

        private const int MenuItemH = 15;
        private const int MenuIndent = 8;
        private const int MenuPadding = 5;
        private static readonly Color backgroundColor = new Color(0, 0, 0, 230);
        private static readonly Color primaryColor = new Color(200, 80, 0);
        private static readonly Color textColor = new Color(0, 205, 255);
        private static readonly Color activeTextColor = new Color(0, 255, 0);

        public delegate T GetValueDelegate<T>();
        public delegate void SetValueDelegate<T>(T value);
        public delegate string GetStringForEnum<T>(T value) where T : Enum;

        private static readonly IFont font = fonts[0];
        public static bool test = false;
        public static int test2 = 0;
        public static float test3 = 0;

        public static bool open = true;
        public static int menuX = 300;
        public static int menuY = 100;
        public static int selectedIndex = 0;
        public static int MaxIndex = 0;
        private static int maxMenuX = 0;
        private static IMenuItem selectedItem = null;
        private static EspModes espmodes;

        private static readonly List<IMenuItem> menuItems = new List<IMenuItem>();

        /// <summary>
        /// generate the static menu
        /// </summary>
        static Menu()
        {

            Category esp = new Category("ESP");

            Category esp_player = new Category("Player");
            esp_player.AddChild(new EnumValue<EspModes>("Mode", () => settings.esp_mode, v => settings.esp_mode = v, EspModesStrings));
            esp_player.AddChild(new EnumValue<EspTarget>("Target", () => settings.esp_target, v => settings.esp_target = v, EspTargetStrings));
            esp_player.AddChild(new Togle("Box", () => settings.esp_box, v => settings.esp_box = v));
            esp_player.AddChild(new Togle("Skeleton", () => settings.esp_skeleton, v => settings.esp_skeleton = v));
            esp_player.AddChild(new Togle("Snaplines", () => settings.esp_snaplines, v => settings.esp_snaplines = v));
            esp_player.AddChild(new Togle("Name", () => settings.esp_name, v => settings.esp_name = v));
            esp_player.AddChild(new Togle("Health", () => settings.esp_health_num, v => settings.esp_health_num = v));
            esp_player.AddChild(new Togle("Health Bar", () => settings.esp_health_bar, v => settings.esp_health_bar = v));
            //esp_player.AddChild(new Togle("Weapon", () => settings.esp_weapon, v => settings.esp_weapon = v));
            //esp_player.AddChild(new Togle("Out Of FOV", () => settings.esp_oov_arrow, v => settings.esp_oov_arrow = v));
            //esp_player.AddChild(new Togle("Flags", () => settings.esp_flags, v => settings.esp_flags = v));
            esp.AddChild(esp_player);

            Category esp_projectiles = new Category("Projectiles");
            esp_projectiles.AddChild(new Togle("Grenade", () => settings.esp_grenade, v => settings.esp_grenade = v));
            esp_projectiles.AddChild(new Togle("Grenade Launcher", () => settings.esp_grenade_launcher, v => settings.esp_grenade_launcher = v));
            esp_projectiles.AddChild(new Togle("Scanner", () => settings.esp_scanner, v => settings.esp_scanner = v));
            esp_projectiles.AddChild(new Togle("C4", () => settings.esp_c4, v => settings.esp_c4 = v));
            esp_projectiles.AddChild(new Togle("Disruptor", () => settings.esp_disruptor, v => settings.esp_disruptor = v));
            esp.AddChild(esp_projectiles);

            menuItems.Add(esp);

            Category aim = new Category("Aim");
            aim.AddChild(new EnumValue<AimbotMode>("Mode", () => settings.aimbot_mode, v => settings.aimbot_mode = v, AimbotModeStrings));

            menuItems.Add(aim);

        }

        /// <summary>
        /// handles key inputs
        /// </summary>
        /// <param name="keyCode">the key pressed</param>
        /// <returns>if the key was used and should not be passed further down the chain</returns>
        public static bool HandleKey(Keys keyCode)
        {
            if(keyCode == menuKey)
            {
                open = !open;
                return true;
            }
            if (open)
            {
                switch (keyCode)
                {
                    case Keys.Up:
                        if (selectedIndex > 0) selectedIndex--;
                        return true;
                    case Keys.Down:
                        if (selectedIndex < MaxIndex) selectedIndex++;
                        return true;
                    default:
                        if (selectedItem != null && selectedItem.ProcessKey(keyCode))
                            return true;
                        break;
                }
                //Log.Debug(keyCode);
            }
            return false;
        }

        /// <summary>
        /// draw the menu
        /// </summary>
        public static void Draw()
        {
            if (!open)
                return;
            //DrawRectFilled(menuX, menuY, 100, 300, backgroundColor);

            int y = menuY + MenuPadding * 2 + font.Size;
            int x = menuX + MenuPadding;
            int index = 0;
            maxMenuX = 0;
            VtxIdxPtr backRect = ReserveRect(true);
            foreach (IMenuItem item in menuItems)
            {
                item.Draw(x, y, out int yo, ref index);
                y += yo;
            }
            MaxIndex = index;
            int w = maxMenuX + MenuPadding - menuX;
            int h = y - menuY;

            DrawRectFilled(menuX, menuY, w, h + MenuPadding, backgroundColor, backRect);
            DrawRect(menuX, menuY, w, h + MenuPadding, 2, primaryColor);
            DrawRectFilled(menuX, menuY + MenuPadding + font.Size, w, 2, primaryColor);

            DrawTextOutlined("nezu.cc", menuX + w / 2, menuY + MenuPadding / 2 + 1, primaryColor, Drawing.TextAlign.TOP | Drawing.TextAlign.H_CENTER);


        }

        class Togle : IMenuItem
        {

            public string name;

            private readonly GetValueDelegate<bool> getValue;

            private readonly SetValueDelegate<bool> setValue;

            public Togle(string name, GetValueDelegate<bool> getValue, SetValueDelegate<bool> setValue)
            {
                this.name = name;
                this.getValue = getValue;
                this.setValue = setValue;
            }

            public int Draw(int startX, int startY, out int yOffset, ref int index)
            {
                if (index == selectedIndex)
                    selectedItem = this;
                index++;
                string label = name + (getValue() ? " 1" : " 0");
                int endX = startX + CalcTextWidth(label, font);
                if (endX > maxMenuX)
                    maxMenuX = endX;
                DrawTextOutlined(label, startX, startY, selectedItem == this ? activeTextColor : textColor);

                yOffset = MenuItemH;
                return 1;
            }

            public bool ProcessKey(Keys key)
            {
                switch (key)
                {
                    case Keys.Right:
                    case Keys.Left:
                    case Keys.Enter:
                        setValue(!getValue());
                        return true;
                }
                return false;
            }
        }

        class EnumValue<T> : IMenuItem where T : Enum, IConvertible
        {

            public string name;

            private readonly GetValueDelegate<T> getValue;

            private readonly SetValueDelegate<T> setValue;

            private readonly GetStringForEnum<T> getStringValue;

            private readonly int min;

            private readonly int max;

            public EnumValue(string name, GetValueDelegate<T> getValue, SetValueDelegate<T> setValue, GetStringForEnum<T> getStringValue)
            {
                this.name = name;
                this.getValue = getValue;
                this.setValue = setValue;
                this.getStringValue = getStringValue;
                min = Enum.GetValues(typeof(T)).Cast<int>().Min();
                max = Enum.GetValues(typeof(T)).Cast<int>().Max();
            }

            public int Draw(int startX, int startY, out int yOffset, ref int index)
            {
                if (index == selectedIndex)
                    selectedItem = this;
                index++;
                string label = name + " " + getStringValue(getValue());
                int endX = startX + CalcTextWidth(label, font);
                if (endX > maxMenuX)
                    maxMenuX = endX;
                DrawTextOutlined(label, startX, startY, selectedItem == this ? activeTextColor : textColor);

                yOffset = MenuItemH;
                return 1;
            }

            public bool ProcessKey(Keys key)
            {
                switch (key)
                {
                    case Keys.Right:
                        {
                            int newVal = getValue().ToInt32(null) + 1;
                            if (newVal < min) newVal = min;
                            if (newVal > max) newVal = max;
                            setValue((T)(object)newVal);
                            return true;
                        }
                    case Keys.Left:
                        {
                            int newVal = getValue().ToInt32(null) - 1;
                            if (newVal < min) newVal = min;
                            if (newVal > max) newVal = max;
                            setValue((T)(object)newVal);
                            return true;
                        }
                }
                return false;
            }
        }

        class IntValue : IMenuItem
        {

            public string name;

            private readonly GetValueDelegate<int> getValue;

            private readonly SetValueDelegate<int> setValue;

            private readonly int min;

            private readonly int max;

            private readonly int step;

            public IntValue(string name, GetValueDelegate<int> getValue, SetValueDelegate<int> setValue, 
                int min = 0, int max = int.MaxValue, int step = 1)
            {
                this.name = name;
                this.getValue = getValue;
                this.setValue = setValue;
                this.min = min;
                this.max = max;
                this.step = step;
            }

            public int Draw(int startX, int startY, out int yOffset, ref int index)
            {
                if (index == selectedIndex)
                    selectedItem = this;
                index++;
                string label = name + " " + getValue();
                int endX = startX + CalcTextWidth(label, font);
                if (endX > maxMenuX)
                    maxMenuX = endX;
                DrawTextOutlined(label, startX, startY, selectedItem == this ? activeTextColor : textColor);

                yOffset = MenuItemH;
                return 1;
            }

            public bool ProcessKey(Keys key)
            {
                switch (key)
                {
                    case Keys.Right:
                    {
                        int newVal = getValue() + step;
                        if (newVal < min) newVal = min;
                        if (newVal > max) newVal = max;
                        setValue(newVal);
                        return true;
                    }
                    case Keys.Left:
                    {
                        int newVal = getValue() - step;
                        if (newVal < min) newVal = min;
                        if (newVal > max) newVal = max;
                        setValue(newVal);
                        return true;
                    }
                }
                return false;
            }
        }

        class FloatValue : IMenuItem
        {

            public string name;

            private readonly GetValueDelegate<float> getValue;

            private readonly SetValueDelegate<float> setValue;

            private readonly float min;

            private readonly float max;

            private readonly float step;

            private readonly int precision;

            public FloatValue(string name, GetValueDelegate<float> getValue, SetValueDelegate<float> setValue,
                float min = 0, float max = float.MaxValue, float step = 1, int precision = 1)
            {
                this.name = name;
                this.getValue = getValue;
                this.setValue = setValue;
                this.min = min;
                this.max = max;
                this.step = step;
                this.precision = precision;
            }

            public int Draw(int startX, int startY, out int yOffset, ref int index)
            {
                if (index == selectedIndex)
                    selectedItem = this;
                index++;
                string label = name + " " + getValue().ToString("F" + precision);
                int endX = startX + CalcTextWidth(label, font);
                if (endX > maxMenuX)
                    maxMenuX = endX;
                DrawTextOutlined(label, startX, startY, selectedItem == this ? activeTextColor : textColor);

                yOffset = MenuItemH;
                return 1;
            }

            public bool ProcessKey(Keys key)
            {
                switch (key)
                {
                    case Keys.Right:
                        {
                            float newVal = getValue() + step;
                            if (newVal < min) newVal = min;
                            if (newVal > max) newVal = max;
                            setValue(newVal);
                            return true;
                        }
                    case Keys.Left:
                        {
                            float newVal = getValue() - step;
                            if (newVal < min) newVal = min;
                            if (newVal > max) newVal = max;
                            setValue(newVal);
                            return true;
                        }
                }
                return false;
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

            public int Draw(int startX, int startY, out int yOffset, ref int index)
            {
                if (index == selectedIndex)
                    selectedItem = this;
                index++;
                string label = (expanded ? "[+] " : "[-] ") + name;
                int endX = startX + CalcTextWidth(label, font);
                if (endX > maxMenuX)
                    maxMenuX = endX;
                DrawTextOutlined(label, startX, startY, selectedItem == this ? activeTextColor : textColor);
                int drawn = 1; //Category header

                int x = startX + MenuIndent; // add title x offset
                int y = startY + MenuItemH; //indenyt children

                if (expanded)
                {
                    foreach (IMenuItem item in _children)
                    {
                        drawn += item.Draw(x, y, out int yo, ref index);
                        y += yo;
                    }
                }

                yOffset = y - startY;
                return drawn;
            }

            public void AddChild(IMenuItem item) => _children.Add(item);

            public IMenuItem[] GetChildren() => _children.ToArray();

            public bool ProcessKey(Keys key)
            {
                switch (key)
                {
                    case Keys.Right:
                    case Keys.Left: 
                    case Keys.Enter:
                        expanded = !expanded; 
                        return true;
                }
                return false;
            }
        }

        interface IMenuItem
        {
            int Draw(int startX, int startY, out int yOffset, ref int index);

            bool ProcessKey(Keys key);

        }

    }
}
