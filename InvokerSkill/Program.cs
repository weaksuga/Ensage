using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using SharpDX;
using SharpDX.Direct3D9;

namespace InvokerSkill
{
    internal class Program
    {
        #region Members

        private static bool _loaded;
        private static bool skilltrue = false;
        private static readonly Dictionary<string, SpellStruct> SpellInfo = new Dictionary<string, SpellStruct>();
        private static Ability q, w, e, ss, coldsnap, ghostwalk, icewall, tornado, deafblast, forge, emp, alacrity, chaosmeteor;
        private static Ability[] spell = new Ability[11];
        private static int startspell = 10;
        private static Vector2 _sizer = new Vector2(265, 300);
        private static Hero me;
        private static Font FontArray;
        public static Dictionary<string, DotaTexture> _textureCache = new Dictionary<string, DotaTexture>();
        public static Dictionary<Ability, string> _skillicon = new Dictionary<Ability, string>();
        public static float startposx = 653, startposy = 810;
        #endregion

        #region ChangeKeyOrSkill
        //Change Hotkey here
        private static char firstkey = '1';
        private static char secondkey = '2';
        private static char thirdkey = '3';
        private static char forthkey = '4';
        private static char fifthkey = '5';
        private static char sixthkey = '6';
        private static char seventhkey = '7';
        private static char eighthkey = '8';
        private static char ninthkey = '9';
        private static char tenthkey = '0';
        //Change skill position here
        private static Ability firstskill = tornado;
        private static Ability secondskill = chaosmeteor;
        private static Ability thirdskill = emp;
        private static Ability forthskill = deafblast;
        private static Ability fifthskill = coldsnap;
        private static Ability sixthskill = ghostwalk;
        private static Ability seventhskill = forge;
        private static Ability eighthskill = icewall;
        private static Ability ninthskill = alacrity;
        private static Ability tenthskill = ss;
        #endregion

        private static void Main()
        {
            FontArray = new Font(
                    Drawing.Direct3DDevice9,
                    new FontDescription
                    {
                        FaceName = "Tahoma",
                        Height = 15,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default
                    });
            _skillicon.Add(tornado, "materials/ensage_ui/spellicons/invoker_tornado.vmat");
            _skillicon.Add(chaosmeteor, "materials/ensage_ui/spellicons/invoker_chaos_meteor.vmat");
            _skillicon.Add(emp, "materials/ensage_ui/spellicons/invoker_emp.vmat");
            _skillicon.Add(deafblast, "materials/ensage_ui/spellicons/invoker_deafening_blast.vmat");
            _skillicon.Add(coldsnap, "materials/ensage_ui/spellicons/invoker_cold_snap.vmat");
            _skillicon.Add(ghostwalk, "materials/ensage_ui/spellicons/invoker_ghost_walk.vmat");
            _skillicon.Add(forge, "materials/ensage_ui/spellicons/invoker_forge_spirit.vmat");
            _skillicon.Add(icewall, "materials/ensage_ui/spellicons/invoker_ice_wall.vmat");
            _skillicon.Add(alacrity, "materials/ensage_ui/spellicons/invoker_alacrity.vmat");
            _skillicon.Add(ss, "materials / ensage_ui / spellicons / invoker_sun_strike.vmat");
            Game.OnUpdate += Game_OnUpdate;
            _loaded = false;
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }
        struct SpellStruct
        {
            private readonly Ability _oneAbility;
            private readonly Ability _twoAbility;
            private readonly Ability _threeAbility;

            public SpellStruct(Ability oneAbility, Ability twoAbility, Ability threeAbility)
            {
                _oneAbility = oneAbility;
                _twoAbility = twoAbility;
                _threeAbility = threeAbility;
            }

            public Ability[] GetNeededAbilities()
            {
                return new[] { _oneAbility, _twoAbility, _threeAbility };
            }
        }
        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (Game.IsChatOpen)
                return;
            if (args.WParam == firstkey)
                startspell = 1;
            else if (args.WParam == secondkey)
                startspell = 2;
            else if (args.WParam == thirdkey)
                startspell = 3;
            else if (args.WParam == forthkey)
                startspell = 4;
            else if (args.WParam == fifthkey)
                startspell = 5;
            else if (args.WParam == sixthkey)
                startspell = 6;
            else if (args.WParam == seventhkey)
                startspell = 7;
            else if (args.WParam == eighthkey)
                startspell = 8;
            else if (args.WParam == ninthkey)
                startspell = 9;
            else if (args.WParam == tenthkey)
                startspell = 0;
            else startspell = 10;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            #region Init
            me = ObjectMgr.LocalHero;


            if (!_loaded)
            {
                if (!Game.IsInGame || me == null || me.ClassID != ClassID.CDOTA_Unit_Hero_Invoker)
                {
                    return;
                }
                _loaded = true;

            }

            if (!Game.IsInGame || me == null)
            {
                _loaded = false;
                PrintInfo("> Invorker unLoaded");
                return;
            }

            if (Game.IsPaused)
            {
                return;
            }

            #endregion



            if (startspell < 10)
            {

                if (!skilltrue)
                {
                    skilltrue = true;
                    q = me.Spellbook.SpellQ;
                    w = me.Spellbook.SpellW;
                    e = me.Spellbook.SpellE;

                    ss = me.FindSpell("invoker_sun_strike");
                    coldsnap = me.FindSpell("invoker_cold_snap");
                    ghostwalk = me.FindSpell("invoker_ghost_walk");
                    icewall = me.FindSpell("invoker_ice_wall");
                    tornado = me.FindSpell("invoker_tornado");
                    deafblast = me.FindSpell("invoker_deafening_blast");
                    forge = me.FindSpell("invoker_forge_spirit");
                    emp = me.FindSpell("invoker_emp");
                    alacrity = me.FindSpell("invoker_alacrity");
                    chaosmeteor = me.FindSpell("invoker_chaos_meteor");

                    SpellInfo.Add(ss.Name, new SpellStruct(e, e, e));
                    SpellInfo.Add(coldsnap.Name, new SpellStruct(q, q, q));
                    SpellInfo.Add(ghostwalk.Name, new SpellStruct(q, q, w));
                    SpellInfo.Add(icewall.Name, new SpellStruct(q, q, e));
                    SpellInfo.Add(tornado.Name, new SpellStruct(w, w, q));
                    SpellInfo.Add(deafblast.Name, new SpellStruct(q, w, e));
                    SpellInfo.Add(forge.Name, new SpellStruct(e, e, q));
                    SpellInfo.Add(emp.Name, new SpellStruct(w, w, w));
                    SpellInfo.Add(alacrity.Name, new SpellStruct(w, w, e));
                    SpellInfo.Add(chaosmeteor.Name, new SpellStruct(e, e, w));
                    spell[1] = firstskill;
                    spell[2] = secondskill;
                    spell[3] = thirdskill;
                    spell[4] = forthskill;
                    spell[5] = fifthskill;
                    spell[6] = sixthskill;
                    spell[7] = seventhskill;
                    spell[8] = eighthskill;
                    spell[9] = ninthskill;
                    spell[0] = tenthskill;
                    spell[10] = chaosmeteor; //extra one just for checking condition, will not be used
                }

                SpellStruct s;
                var active1 = me.Spellbook.Spell4;
                var active2 = me.Spellbook.Spell5;
                if (Equals(spell[startspell], active1)) //If the skill inside D
                {
                    spell[startspell].UseAbility();
                    if (Utils.SleepCheck("spell1sleep"))
                    {
                        Game.ExecuteCommand("dota_ability_execute 3");
                        Utils.Sleep(150, "spell1sleep");
                    }

                }
                else if (Equals(spell[startspell], active2)) //If the skill inside F
                {
                    spell[startspell].UseAbility();
                    if (Utils.SleepCheck("spell2sleep"))
                    {
                        Game.ExecuteCommand("dota_ability_execute 4");
                        Utils.Sleep(150, "spell2sleep");
                    }
                }
                else //If not inside D and F, invoke the skill
                {
                    if (SpellInfo.TryGetValue(spell[startspell].Name, out s))
                    {
                        var invoke = me.FindSpell("invoker_invoke");
                        var spells = s.GetNeededAbilities();
                        if (spells[0] != null) spells[0].UseAbility();
                        if (spells[1] != null) spells[1].UseAbility();
                        if (spells[2] != null) spells[2].UseAbility();
                        invoke.UseAbility();
                    }
                }
                startspell = 10;
            }


        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var player = ObjectMgr.LocalPlayer;
            if (player == null || player.Team == Team.Observer || !_loaded)
            {
                return;
            }
            if (ObjectMgr.LocalHero.ClassID != ClassID.CDOTA_Unit_Hero_Invoker) return;

            float[] spellcd = new float[10];
            float[] spelltotalcd = new float[10];
            for (int i = 0; i < 10; i++)
            {
                if (spell[i] == null) continue;
                spellcd[i] = spell[i].Cooldown;
                spelltotalcd[i] = spell[i].CooldownLength;
            }
            Drawing.DrawRect(new Vector2(startposx, startposy), new Vector2(50, 50), GetTexture(_skillicon[firstskill]));
            Drawing.DrawRect(new Vector2(startposx + 50, startposy), new Vector2(50, 50), GetTexture(_skillicon[secondskill]));
            Drawing.DrawRect(new Vector2(startposx + 100, startposy), new Vector2(50, 50), GetTexture(_skillicon[thirdskill]));
            Drawing.DrawRect(new Vector2(startposx + 150, startposy), new Vector2(50, 50), GetTexture(_skillicon[forthskill]));
            Drawing.DrawRect(new Vector2(startposx + 200, startposy), new Vector2(50, 50), GetTexture(_skillicon[fifthskill]));
            Drawing.DrawRect(new Vector2(startposx + 250, startposy), new Vector2(50, 50), GetTexture(_skillicon[sixthskill]));
            Drawing.DrawRect(new Vector2(startposx + 300, startposy), new Vector2(50, 50), GetTexture(_skillicon[seventhskill]));
            Drawing.DrawRect(new Vector2(startposx + 350, startposy), new Vector2(50, 50), GetTexture(_skillicon[eighthskill]));
            Drawing.DrawRect(new Vector2(startposx + 400, startposy), new Vector2(50, 50), GetTexture(_skillicon[ninthskill]));
            Drawing.DrawRect(new Vector2(startposx + 450, startposy), new Vector2(50, 50), GetTexture(_skillicon[tenthskill]));


            //draw box
            Drawing.DrawRect(new Vector2(startposx, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 50, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 100, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 150, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 200, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 250, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 300, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 350, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 400, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);
            Drawing.DrawRect(new Vector2(startposx + 450, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150), true);

            Drawing.DrawRect(new Vector2(startposx, startposy), new Vector2(50, 50 - (1 - (spellcd[1] / spelltotalcd[1])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 50, startposy), new Vector2(50, 50 - (1 - (spellcd[2] / spelltotalcd[2])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 100, startposy), new Vector2(50, 50 - (1 - (spellcd[3] / spelltotalcd[3])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 150, startposy), new Vector2(50, 50 - (1 - (spellcd[4] / spelltotalcd[4])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 200, startposy), new Vector2(50, 50 - (1 - (spellcd[5] / spelltotalcd[5])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 250, startposy), new Vector2(50, 50 - (1 - (spellcd[6] / spelltotalcd[6])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 300, startposy), new Vector2(50, 50 - (1 - (spellcd[7] / spelltotalcd[7])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 350, startposy), new Vector2(50, 50 - (1 - (spellcd[8] / spelltotalcd[8])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 400, startposy), new Vector2(50, 50 - (1 - (spellcd[9] / spelltotalcd[9])) * 50), new Color(255, 255, 255, 70));
            Drawing.DrawRect(new Vector2(startposx + 450, startposy), new Vector2(50, 50 - (1 - (spellcd[0] / spelltotalcd[0])) * 50), new Color(255, 255, 255, 70));

            if (spellcd[1] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[2] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 50, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[3] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 100, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[4] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 150, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[5] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 200, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[6] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 250, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[7] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 300, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[8] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 350, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[9] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 400, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }
            if (spellcd[0] != 0)
            {
                Drawing.DrawRect(new Vector2(startposx + 450, startposy), new Vector2(50, 50), new Color(0, 0, 0, 150));
            }

        }
        static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice9 == null || Drawing.Direct3DDevice9.IsDisposed || !Game.IsInGame)
                return;
            if (ObjectMgr.LocalHero.ClassID != ClassID.CDOTA_Unit_Hero_Invoker) return;
            DrawShadowText("1", (int)startposx + 3, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("2", (int)startposx + 53, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("3", (int)startposx + 103, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("4", (int)startposx + 153, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("5", (int)startposx + 203, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("6", (int)startposx + 253, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("7", (int)startposx + 303, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("8", (int)startposx + 353, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("9", (int)startposx + 403, (int)startposy + 1, Color.LightCyan, FontArray);
            DrawShadowText("0", (int)startposx + 453, (int)startposy + 1, Color.LightCyan, FontArray);
        }
        public static DotaTexture GetTexture(string name)
        {
            if (_textureCache.ContainsKey(name)) return _textureCache[name];

            return _textureCache[name] = Drawing.GetTexture(name);
        }
        public static void DrawShadowText(string stext, int x, int y, Color color, Font f)
        {
            f.DrawText(null, stext, x + 1, y + 1, Color.Black);
            f.DrawText(null, stext, x, y, color);
        }
        #region Helpers
        public static void PrintInfo(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.White, arguments);
        }

        public static void PrintSuccess(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Green, arguments);
        }

        public static void PrintError(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Red, arguments);
        }

        public static void PrintEncolored(string text, ConsoleColor color, params object[] arguments)
        {
            var clr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, arguments);
            Console.ForegroundColor = clr;
        }
        #endregion

    }
}
