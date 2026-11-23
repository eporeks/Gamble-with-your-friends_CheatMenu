using BepInEx;
using HarmonyLib;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestMenu
{
    [BepInPlugin("com.cheat.menu", "InGame Menu", "2.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private bool show = true;
        private Rect rect = new Rect(40, 40, 540, 420);
        private int activeTab = 0;
        private Vector2 scrollPos = Vector2.zero;

        // Флаги состояния функций
        public static bool rigDuckRace = false;
        public static bool zeroCrashChance = false;
        public static bool infiniteCrashPoint = false;
        public static bool rigBlackjack = false;
        public static bool customDragonTower = false;
        public static bool alwaysZeroRoulette = false;
        public static bool customMinesweeper = false;
        public static bool rigMoneyWheel = false;
        public static bool rigPlinko = false;
        public static bool rigSlots = false;

        // Стили и текстуры
        private bool stylesInitialized = false;
        private GUIStyle brandTitleStyle;
        private GUIStyle brandBadgeStyle;
        private GUIStyle sectionTitleStyle;
        private GUIStyle itemTitleStyle;
        private GUIStyle itemDescStyle;
        private GUIStyle tabNormalStyle;
        private GUIStyle tabActiveStyle;
        private GUIStyle actionBtnStyle;
        private GUIStyle footerTextStyle;
        private GUIStyle statusBadgeStyle;

        private Texture2D windowBgTex;
        private Texture2D windowBorderTex;
        private Texture2D sidebarBgTex;
        private Texture2D cardBgTex;
        private Texture2D cardHoverBgTex;
        private Texture2D cardBorderTex;
        private Texture2D cardHoverBorderTex;
        private Texture2D topAccentTex;
        private Texture2D dividerTex;
        private Texture2D indicatorActiveTex;
        private Texture2D indicatorInactiveTex;
        private Texture2D toggleTrackOnTex;
        private Texture2D toggleTrackOffTex;
        private Texture2D toggleKnobTex;

        private readonly string[] tabNames = new string[]
        {
            "  CRASH & ARCADE",
            "  TABLE & SLOTS",
            "  SYSTEM & MISC"
        };

        private void Awake()
        {
            var harmony = new Harmony("com.cheat.menu");
            harmony.PatchAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                show = !show;
            }
        }

        private void InitStyles()
        {
            // Проверяем также windowBgTex == null на случай, если Unity очистила текстуры при смене сцены
            if (stylesInitialized && windowBgTex != null) return;

            Color bgMain = new Color(0.055f, 0.058f, 0.075f, 0.98f);
            Color bgSidebar = new Color(0.040f, 0.043f, 0.056f, 1f);
            Color bgCard = new Color(0.088f, 0.094f, 0.124f, 1f);
            Color bgCardHover = new Color(0.115f, 0.124f, 0.162f, 1f);
            Color borderSubtle = new Color(0.18f, 0.20f, 0.26f, 1f);

            Color accentPrimary = new Color(0.45f, 0.36f, 0.98f, 1f);
            Color accentSecondary = new Color(0.00f, 0.82f, 0.95f, 1f);
            Color tabActiveBg = new Color(0.45f, 0.36f, 0.98f, 0.22f);

            windowBgTex = MakeTex(2, 2, bgMain);
            windowBorderTex = MakeTex(2, 2, borderSubtle);
            sidebarBgTex = MakeTex(2, 2, bgSidebar);
            cardBgTex = MakeTex(2, 2, bgCard);
            cardHoverBgTex = MakeTex(2, 2, bgCardHover);
            cardBorderTex = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.07f));
            cardHoverBorderTex = MakeTex(2, 2, new Color(0.45f, 0.36f, 0.98f, 0.65f));

            topAccentTex = MakeHorizontalGradientTex(256, 3, accentPrimary, accentSecondary);
            dividerTex = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.08f));
            indicatorActiveTex = MakeTex(2, 2, accentPrimary);
            indicatorInactiveTex = MakeTex(2, 2, new Color(0.22f, 0.24f, 0.30f, 0.6f));

            toggleTrackOnTex = MakeRoundedTex(44, 22, 11, accentPrimary, accentPrimary);
            toggleTrackOffTex = MakeRoundedTex(44, 22, 11, new Color(0.15f, 0.16f, 0.21f, 1f), borderSubtle);
            toggleKnobTex = MakeRoundedTex(16, 16, 8, Color.white, Color.white);

            brandTitleStyle = new GUIStyle(GUI.skin.label);
            brandTitleStyle.fontSize = 15;
            brandTitleStyle.fontStyle = FontStyle.Bold;
            brandTitleStyle.normal.textColor = Color.white;

            brandBadgeStyle = new GUIStyle(GUI.skin.label);
            brandBadgeStyle.fontSize = 10;
            brandBadgeStyle.fontStyle = FontStyle.Bold;
            brandBadgeStyle.normal.textColor = accentSecondary;

            sectionTitleStyle = new GUIStyle(GUI.skin.label);
            sectionTitleStyle.fontSize = 11;
            sectionTitleStyle.fontStyle = FontStyle.Bold;
            sectionTitleStyle.normal.textColor = new Color(0.55f, 0.58f, 0.68f, 1f);

            itemTitleStyle = new GUIStyle(GUI.skin.label);
            itemTitleStyle.fontSize = 13;
            itemTitleStyle.fontStyle = FontStyle.Bold;
            itemTitleStyle.normal.textColor = new Color(0.95f, 0.96f, 0.99f, 1f);

            itemDescStyle = new GUIStyle(GUI.skin.label);
            itemDescStyle.fontSize = 11;
            itemDescStyle.normal.textColor = new Color(0.58f, 0.61f, 0.72f, 1f);

            tabNormalStyle = new GUIStyle(GUI.skin.button);
            tabNormalStyle.fontSize = 11;
            tabNormalStyle.fontStyle = FontStyle.Bold;
            tabNormalStyle.alignment = TextAnchor.MiddleLeft;
            tabNormalStyle.normal.textColor = new Color(0.65f, 0.68f, 0.78f, 1f);
            tabNormalStyle.hover.textColor = Color.white;
            tabNormalStyle.normal.background = MakeTex(2, 2, Color.clear);
            tabNormalStyle.hover.background = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.05f));
            tabNormalStyle.fixedHeight = 34;
            tabNormalStyle.margin = new RectOffset(0, 0, 2, 2);

            tabActiveStyle = new GUIStyle(tabNormalStyle);
            tabActiveStyle.normal.textColor = Color.white;
            tabActiveStyle.normal.background = MakeTex(2, 2, tabActiveBg);
            tabActiveStyle.hover.background = tabActiveStyle.normal.background;

            actionBtnStyle = new GUIStyle(GUI.skin.button);
            actionBtnStyle.fontSize = 11;
            actionBtnStyle.fontStyle = FontStyle.Bold;
            actionBtnStyle.normal.textColor = new Color(0.88f, 0.90f, 0.97f, 1f);
            actionBtnStyle.hover.textColor = Color.white;
            actionBtnStyle.normal.background = cardBgTex;
            actionBtnStyle.hover.background = cardHoverBgTex;
            actionBtnStyle.fixedHeight = 30;

            footerTextStyle = new GUIStyle(GUI.skin.label);
            footerTextStyle.fontSize = 10;
            footerTextStyle.normal.textColor = new Color(0.48f, 0.51f, 0.62f, 1f);

            statusBadgeStyle = new GUIStyle(GUI.skin.label);
            statusBadgeStyle.fontSize = 10;
            statusBadgeStyle.fontStyle = FontStyle.Bold;

            stylesInitialized = true;
        }

        private void OnGUI()
        {
            if (!show) return;

            InitStyles();
            // Используем GUIStyle.none и рисуем фон вручную внутри WindowFunc — это гарантирует 100% видимость фона
            rect = GUI.Window(0, rect, WindowFunc, GUIContent.none, GUIStyle.none);
        }

        private void WindowFunc(int id)
        {
            // 1. Внешняя рамка и основной фон окна
            GUI.DrawTexture(new Rect(0, 0, rect.width, rect.height), windowBorderTex);
            GUI.DrawTexture(new Rect(1, 1, rect.width - 2, rect.height - 2), windowBgTex);

            // 2. Фон левого сайдбара и разделительная линия
            float sidebarWidth = 170f;
            GUI.DrawTexture(new Rect(1, 3, sidebarWidth, rect.height - 4), sidebarBgTex);
            GUI.DrawTexture(new Rect(sidebarWidth, 3, 1, rect.height - 4), dividerTex);

            // 3. Верхняя неоновая градиентная полоса
            GUI.DrawTexture(new Rect(0, 0, rect.width, 3), topAccentTex);

            GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));

            // ==========================================
            // ЛЕВАЯ ПАНЕЛЬ (SIDEBAR)
            // ==========================================
            GUILayout.BeginVertical(GUILayout.Width(sidebarWidth), GUILayout.ExpandHeight(true));
            GUILayout.Space(14);

            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginVertical();
            GUILayout.Label("AETHER // PRIV", brandTitleStyle);
            GUILayout.Label("INTERNAL BUILD 2.0", brandBadgeStyle);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(12);
            DrawDivider();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginVertical();
            GUILayout.Label("MODULES", sectionTitleStyle);
            GUILayout.Space(4);

            for (int i = 0; i < tabNames.Length; i++)
            {
                if (GUILayout.Button(tabNames[i], activeTab == i ? tabActiveStyle : tabNormalStyle))
                {
                    activeTab = i;
                    scrollPos = Vector2.zero;
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginVertical();
            bool isHost = NetworkServer.active;
            statusBadgeStyle.normal.textColor = isHost
                ? new Color(0.15f, 0.85f, 0.55f, 1f)
                : new Color(0.95f, 0.65f, 0.20f, 1f);

            GUILayout.Label(isHost ? "● HOST SERVER ACTIVE" : "● CLIENT MODE", statusBadgeStyle);
            GUILayout.Label($"ACTIVE MODS: {CountActiveModules()} / 10", footerTextStyle);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(12);
            GUILayout.EndVertical();

            // ==========================================
            // ПРАВАЯ ПАНЕЛЬ (ОСНОВНОЙ КОНТЕНТ)
            // ==========================================
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Space(14);

            GUILayout.BeginHorizontal();
            GUILayout.Space(16);
            GUILayout.Label(GetTabHeader(activeTab), brandTitleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label("[INS]", footerTextStyle);
            GUILayout.Space(16);
            GUILayout.EndHorizontal();

            GUILayout.Space(8);

            GUILayout.BeginHorizontal();
            GUILayout.Space(14);
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (activeTab == 0)
            {
                DrawModuleCard("Crash — Infinite Multiplier", "Устанавливает точку краша на 9,999,999x", ref infiniteCrashPoint);
                DrawModuleCard("CrossyRoad — God Mode", "Обнуляет шанс падения пингвина на каждом шаге", ref zeroCrashChance);
                DrawModuleCard("Dragon Tower — Safe Path", "Убирает яйца со всех этажей башни", ref customDragonTower);
                DrawModuleCard("Minesweeper — No Mines", "Очищает сетку мин перед началом раунда", ref customMinesweeper);
                DrawModuleCard("Plinko — Max Slot Magnet", "Направляет выплату в лунку с максимальным X", ref rigPlinko);
            }
            else if (activeTab == 1)
            {
                DrawModuleCard("Blackjack — Instant 21", "Подкладывает Туз и 10-очковую карту игроку", ref rigBlackjack);
                DrawModuleCard("Roulette — Zero Rig", "Фиксирует выпадение сектора 0 на колесе", ref alwaysZeroRoulette);
                DrawModuleCard("MoneyWheel — 10x Orange", "Автоставка на Orange и гарантированный 10x", ref rigMoneyWheel);
                DrawModuleCard("Slots — Force Jackpot", "Активирует все выигрышные линии с топ-символом", ref rigSlots);
                DrawModuleCard("Duck Race — Turbo Win", "Ускоряет выбранную утку и замедляет соперников", ref rigDuckRace);
            }
            else if (activeTab == 2)
            {
                GUILayout.Label("QUICK ACTIONS", sectionTitleStyle);
                GUILayout.Space(6);

                if (GUILayout.Button("DISABLE ALL MODULES", actionBtnStyle))
                {
                    DisableAll();
                }

                GUILayout.Space(6);

                if (GUILayout.Button("ENABLE ALL MODULES", actionBtnStyle))
                {
                    EnableAll();
                }

                GUILayout.Space(14);
                GUILayout.Label("INFORMATION", sectionTitleStyle);
                GUILayout.Space(4);
                GUILayout.Label("• Все серверные эксплойты требуют прав хоста лобби.", itemDescStyle);
                GUILayout.Label("• Клавиша INSERT скрывает или открывает оверлей.", itemDescStyle);
                GUILayout.Label("• Перетаскивание окна доступно за верхнюю шапку.", itemDescStyle);
            }

            GUILayout.EndScrollView();
            GUILayout.Space(12);
            GUILayout.EndHorizontal();

            GUILayout.Space(12);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0, 0, rect.width, 42));
        }

        private void DrawModuleCard(string title, string description, ref bool state)
        {
            Rect cardRect = GUILayoutUtility.GetRect(100, 54, GUILayout.ExpandWidth(true));
            bool isHovered = cardRect.Contains(Event.current.mousePosition);

            // Рамка и фон карточки через GUI.DrawTexture
            GUI.DrawTexture(cardRect, isHovered ? cardHoverBorderTex : cardBorderTex);
            GUI.DrawTexture(new Rect(cardRect.x + 1, cardRect.y + 1, cardRect.width - 2, cardRect.height - 2), isHovered ? cardHoverBgTex : cardBgTex);

            // Вертикальная полоска-индикатор слева
            Rect indicatorRect = new Rect(cardRect.x + 1, cardRect.y + 10, 3, cardRect.height - 20);
            GUI.DrawTexture(indicatorRect, state ? indicatorActiveTex : indicatorInactiveTex);

            // Текст заголовка и описания
            Rect titleRect = new Rect(cardRect.x + 14, cardRect.y + 8, cardRect.width - 74, 20);
            Rect descRect = new Rect(cardRect.x + 14, cardRect.y + 27, cardRect.width - 74, 18);

            GUI.Label(titleRect, title, itemTitleStyle);
            GUI.Label(descRect, description, itemDescStyle);

            // Кастомный Pill-переключатель справа
            Rect toggleRect = new Rect(cardRect.xMax - 56, cardRect.y + (cardRect.height - 22) * 0.5f, 44, 22);
            GUI.DrawTexture(toggleRect, state ? toggleTrackOnTex : toggleTrackOffTex);

            float knobX = state ? (toggleRect.xMax - 19) : (toggleRect.x + 3);
            Rect knobRect = new Rect(knobX, toggleRect.y + 3, 16, 16);
            GUI.DrawTexture(knobRect, toggleKnobTex);

            // Клик по карточке
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && cardRect.Contains(Event.current.mousePosition))
            {
                state = !state;
                Event.current.Use();
            }

            GUILayout.Space(6);
        }

        private string GetTabHeader(int tab)
        {
            switch (tab)
            {
                case 0: return "Crash & Arcade Exploits";
                case 1: return "Table, Wheels & Slots";
                default: return "Configuration & Status";
            }
        }

        private int CountActiveModules()
        {
            int count = 0;
            if (rigDuckRace) count++;
            if (zeroCrashChance) count++;
            if (infiniteCrashPoint) count++;
            if (rigBlackjack) count++;
            if (customDragonTower) count++;
            if (alwaysZeroRoulette) count++;
            if (customMinesweeper) count++;
            if (rigMoneyWheel) count++;
            if (rigPlinko) count++;
            if (rigSlots) count++;
            return count;
        }

        private void DisableAll()
        {
            rigDuckRace = false;
            zeroCrashChance = false;
            infiniteCrashPoint = false;
            rigBlackjack = false;
            customDragonTower = false;
            alwaysZeroRoulette = false;
            customMinesweeper = false;
            rigMoneyWheel = false;
            rigPlinko = false;
            rigSlots = false;
        }

        private void EnableAll()
        {
            rigDuckRace = true;
            zeroCrashChance = true;
            infiniteCrashPoint = true;
            rigBlackjack = true;
            customDragonTower = true;
            alwaysZeroRoulette = true;
            customMinesweeper = true;
            rigMoneyWheel = true;
            rigPlinko = true;
            rigSlots = true;
        }

        private void DrawDivider()
        {
            Rect r = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
            GUI.DrawTexture(r, dividerTex);
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
            Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.hideFlags = HideFlags.HideAndDontSave;
            result.wrapMode = TextureWrapMode.Clamp;
            result.filterMode = FilterMode.Point;
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private Texture2D MakeHorizontalGradientTex(int width, int height, Color left, Color right)
        {
            Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.hideFlags = HideFlags.HideAndDontSave;
            result.wrapMode = TextureWrapMode.Clamp;
            result.filterMode = FilterMode.Bilinear;
            for (int x = 0; x < width; x++)
            {
                Color c = Color.Lerp(left, right, (float)x / (width - 1));
                for (int y = 0; y < height; y++)
                {
                    result.SetPixel(x, y, c);
                }
            }
            result.Apply();
            return result;
        }

        private Texture2D MakeRoundedTex(int width, int height, int radius, Color fill, Color border)
        {
            Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.hideFlags = HideFlags.HideAndDontSave;
            result.wrapMode = TextureWrapMode.Clamp;
            result.filterMode = FilterMode.Bilinear;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dist = RoundedBoxSDF(
                        new Vector2(x - width * 0.5f + 0.5f, y - height * 0.5f + 0.5f),
                        new Vector2(width * 0.5f - 0.5f, height * 0.5f - 0.5f),
                        radius
                    );

                    if (dist > 0.5f)
                    {
                        result.SetPixel(x, y, Color.clear);
                    }
                    else if (dist > -1.0f && border.a > 0f)
                    {
                        float alpha = Mathf.Clamp01(0.5f - dist);
                        Color col = Color.Lerp(fill, border, 1f - Mathf.Clamp01(-dist));
                        col.a *= alpha;
                        result.SetPixel(x, y, col);
                    }
                    else
                    {
                        result.SetPixel(x, y, fill);
                    }
                }
            }
            result.Apply();
            return result;
        }

        private float RoundedBoxSDF(Vector2 p, Vector2 b, float r)
        {
            Vector2 q = new Vector2(Mathf.Abs(p.x), Mathf.Abs(p.y)) - b + new Vector2(r, r);
            return Mathf.Min(Mathf.Max(q.x, q.y), 0f) + Vector2.Max(q, Vector2.zero).magnitude - r;
        }
    }

    // ==========================================
    // ПАТЧ 1: CrossyRoad — GetCrashChanceForStep
    // ==========================================
    [HarmonyPatch(typeof(CrossyRoad), "GetCrashChanceForStep")]
    public static class CrashChancePatch
    {
        static bool Prefix(ref float __result)
        {
            if (!Plugin.zeroCrashChance) return true;

            __result = 0f;
            return false;
        }
    }

    // ==========================================
    // ПАТЧ 2: Crash — SetCrashPoint
    // ==========================================
    [HarmonyPatch(typeof(Crash), "SetCrashPoint")]
    public static class SetCrashPointPatch
    {
        static bool Prefix(Crash __instance)
        {
            if (!Plugin.infiniteCrashPoint) return true;

            float crashPoint = 9999999f;

            var raiseRoutineMethod = AccessTools.Method(typeof(Crash), "RaiseRoutine");
            if (raiseRoutineMethod == null)
            {
                Debug.LogError("[InGame Menu] Метод Crash.RaiseRoutine не найден!");
                return true;
            }

            IEnumerator routine = (IEnumerator)raiseRoutineMethod.Invoke(__instance, new object[] { crashPoint });
            __instance.StartCoroutine(routine);

            var hasStartedProp = AccessTools.Property(typeof(Crash), "Network_hasStarted");
            if (hasStartedProp != null)
            {
                hasStartedProp.SetValue(__instance, true);
            }
            else
            {
                var hasStartedField = AccessTools.Field(typeof(Crash), "_hasStarted");
                if (hasStartedField != null) hasStartedField.SetValue(__instance, true);
            }

            return false;
        }
    }

    // ==========================================
    // ПАТЧ 3: Blackjack — DealCardToPlayer
    // ==========================================
    [HarmonyPatch(typeof(Blackjack), "DealCardToPlayer")]
    public static class BlackjackDealPatch
    {
        static bool Prefix(Blackjack __instance)
        {
            if (!Plugin.rigBlackjack) return true;
            if (!NetworkServer.active) return true;

            var deckField = AccessTools.Field(typeof(Blackjack), "deck");
            var playerHandField = AccessTools.Field(typeof(Blackjack), "playerHand");
            var initDeckMethod = AccessTools.Method(typeof(Blackjack), "InitializeDeck");

            var deck = deckField?.GetValue(__instance) as List<CardData>;
            // Используем IList (или SyncList<CardData>), так как SyncList реализует IList
            var playerHand = playerHandField?.GetValue(__instance) as IList;

            if (deck == null || playerHand == null) return true;

            // Если колода пуста, инициализируем её заранее, чтобы было из чего выбирать
            if (deck.Count == 0)
            {
                initDeckMethod?.Invoke(__instance, null);
            }

            int targetIndex = -1;
            if (playerHand.Count == 0)
            {
                // Первая карта — Туз (11 очков)
                targetIndex = deck.FindIndex(c => c.Rank == Rank.Ace);
            }
            else if (playerHand.Count == 1)
            {
                // Вторая карта — любая карта номиналом 10 (10, Валет, Дама, Король)
                targetIndex = deck.FindIndex(c => c.GetBlackjackValue() == 10);
            }

            // Перемещаем найденную карту на верх колоды (индекс 0)
            if (targetIndex > 0)
            {
                CardData card = deck[targetIndex];
                deck.RemoveAt(targetIndex);
                deck.Insert(0, card);
            }

            // Возвращаем true — оригинальный DealCardToPlayer сам возьмёт deck[0] и раздаст игроку
            return true;
        }
    }

    // ==========================================
    // ПАТЧ 4: DuckRace — DuckRaceRoutine
    // ==========================================
    [HarmonyPatch(typeof(DuckRaceDuck), "DuckRaceRoutine")]
    public static class DuckRacePatch
    {
        static bool Prefix(DuckRaceDuck __instance, System.Random rng, ref IEnumerator __result)
        {
            if (!Plugin.rigDuckRace) return true;

            __result = RiggedDuckRaceRoutine(__instance, rng);
            return false;
        }

        private static IEnumerator RiggedDuckRaceRoutine(DuckRaceDuck instance, System.Random rng)
        {
            var duckRaceField = AccessTools.Field(typeof(DuckRaceDuck), "duckRace");
            var minStepDelayField = AccessTools.Field(typeof(DuckRaceDuck), "minStepDelay");
            var maxStepDelayField = AccessTools.Field(typeof(DuckRaceDuck), "maxStepDelay");
            var minStepDistanceField = AccessTools.Field(typeof(DuckRaceDuck), "minStepDistance");
            var maxStepDistanceField = AccessTools.Field(typeof(DuckRaceDuck), "maxStepDistance");
            var stepTweenDurationField = AccessTools.Field(typeof(DuckRaceDuck), "stepTweenDuration");

            var ducksField = AccessTools.Field(typeof(DuckRace), "ducks");
            var betIndexField = AccessTools.Field(typeof(DuckRace), "_placedBetIndex");

            var rpcSetRunningAnim = AccessTools.Method(typeof(DuckRaceDuck), "RpcSetRunningAnimation");
            var rpcStep = AccessTools.Method(typeof(DuckRaceDuck), "RpcStep");
            var rpcWinFeedback = AccessTools.Method(typeof(DuckRaceDuck), "RpcWinFeedback");

            DuckRace duckRace = (DuckRace)duckRaceField.GetValue(instance);
            float minStepDelay = (float)minStepDelayField.GetValue(instance);
            float maxStepDelay = (float)maxStepDelayField.GetValue(instance);
            float minStepDistance = (float)minStepDistanceField.GetValue(instance);
            float maxStepDistance = (float)maxStepDistanceField.GetValue(instance);
            float stepTweenDuration = (float)stepTweenDurationField.GetValue(instance);

            rpcSetRunningAnim.Invoke(instance, new object[] { true });

            while (!duckRace.hasEnded)
            {
                List<DuckRaceDuck> ducksList = (List<DuckRaceDuck>)ducksField.GetValue(duckRace);
                int betIndex = (int)betIndexField.GetValue(duckRace);
                bool isPlayerDuck = (ducksList.IndexOf(instance) == betIndex);

                float num = isPlayerDuck ? (maxStepDistance * 2f) : (minStepDistance * 0.1f);
                float seconds = isPlayerDuck ? 0.01f : (maxStepDelay * 2f);

                float targetZ = Mathf.Min(instance.transform.localPosition.z + num, duckRace.endPoint.localPosition.z);
                rpcStep.Invoke(instance, new object[] { targetZ });
                yield return new WaitForSeconds(stepTweenDuration);

                if (Mathf.Approximately(targetZ, duckRace.endPoint.localPosition.z))
                {
                    if (duckRace.OnDuckFinish(instance))
                    {
                        rpcWinFeedback.Invoke(instance, null);
                    }
                    rpcSetRunningAnim.Invoke(instance, new object[] { false });
                    yield break;
                }

                yield return new WaitForSeconds(seconds);
            }

            rpcSetRunningAnim.Invoke(instance, new object[] { false });
            yield break;
        }
    }

    // ==========================================
    // ПАТЧ 5: DragonTower — SetEggs & RevealEggs
    // ==========================================
    [HarmonyPatch(typeof(DragonTower), "SetEggs")]
    public static class SetEggsPatch
    {
        static bool Prefix(DragonTower __instance)
        {
            if (!Plugin.customDragonTower) return true;

            var floorsField = AccessTools.Field(typeof(DragonTower), "floors");
            var floors = floorsField?.GetValue(__instance) as IEnumerable;

            if (floors != null)
            {
                var floorType = AccessTools.Inner(typeof(DragonTower), "Floor");
                var eggIndexField = AccessTools.Field(floorType, "eggIndex");

                foreach (var floor in floors)
                {
                    eggIndexField?.SetValue(floor, -1);
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(DragonTower), "RevealEggs")]
    public static class RevealEggsPatch
    {
        static bool Prefix(DragonTower __instance)
        {
            if (!Plugin.customDragonTower) return true;

            var floorsField = AccessTools.Field(typeof(DragonTower), "floors");
            var currentFloorField = AccessTools.Field(typeof(DragonTower), "_currentFloor");

            var floors = floorsField?.GetValue(__instance) as IList;
            int currentFloor = (int)(currentFloorField?.GetValue(__instance) ?? 0);

            if (floors != null)
            {
                var floorType = AccessTools.Inner(typeof(DragonTower), "Floor");
                var eggIndexField = AccessTools.Field(floorType, "eggIndex");
                var buttonsField = AccessTools.Field(floorType, "buttons");

                for (int i = 0; i < currentFloor && i < floors.Count; i++)
                {
                    var floor = floors[i];
                    int eggIndex = (int)eggIndexField.GetValue(floor);
                    var buttons = buttonsField.GetValue(floor) as IList;

                    if (buttons != null && eggIndex >= 0 && eggIndex < buttons.Count)
                    {
                        var btn = buttons[eggIndex] as DragonTowerButton;
                        btn?.ServerSetButtonState(DragonTowerButton.ButtonState.RevealEgg);
                    }
                }
            }

            return false;
        }
    }

    // ==========================================
    // ПАТЧ 6: Roulette — StartGame & FixedRandom
    // ==========================================
    public class FixedRandom : System.Random
    {
        private readonly int _fixedValue;

        public FixedRandom(int fixedValue)
        {
            _fixedValue = fixedValue;
        }

        public override int Next(int minValue, int maxValue)
        {
            if (_fixedValue < minValue) return minValue;
            if (_fixedValue >= maxValue) return maxValue - 1;
            return _fixedValue;
        }
    }

    [HarmonyPatch(typeof(Roulette), "StartGame")]
    public static class RouletteStartGamePatch
    {
        static bool Prefix(Roulette __instance)
        {
            if (!Plugin.alwaysZeroRoulette) return true;
            if (!NetworkServer.active) return false;

            BaseStartGameReversePatch.CallBaseStartGame(__instance);

            var wheelField = AccessTools.Field(typeof(Roulette), "wheel");
            var wheel = wheelField?.GetValue(__instance) as Wheel;

            if (wheel != null)
            {
                wheel.SpinTheWheel(new FixedRandom(0));
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(GameBase), "StartGame")]
    public static class BaseStartGameReversePatch
    {
        [HarmonyReversePatch]
        public static void CallBaseStartGame(GameBase instance) => throw new NotImplementedException();
    }

    // ==========================================
    // ПАТЧ 7: Minesweeper — SetMines
    // ==========================================
    [HarmonyPatch(typeof(Minesweeper), "SetMines")]
    public static class SetMinesPatch
    {
        static bool Prefix(Minesweeper __instance)
        {
            if (!Plugin.customMinesweeper) return true;
            if (!NetworkServer.active) return false;

            var mineIndexesField = AccessTools.Field(typeof(Minesweeper), "_mineIndexes");
            var mineIndexes = mineIndexesField?.GetValue(__instance) as HashSet<int>;

            mineIndexes?.Clear();

            return false;
        }
    }

    // ==========================================
    // ПАТЧ 8: MoneyWheel — HandleWheelStopped
    // ==========================================
    [HarmonyPatch(typeof(MoneyWheel), "HandleWheelStopped")]
    public static class MoneyWheelPatch
    {
        static bool Prefix(MoneyWheel __instance, ref string result)
        {
            if (!Plugin.rigMoneyWheel) return true;

            result = "Orange";

            var betOptionField = AccessTools.Field(typeof(MoneyWheel), "_currentBettingOption");
            betOptionField?.SetValue(__instance, "Orange");

            return true;
        }
    }

    // ==========================================
    // ПАТЧ 9: Plinko — OnPuckEnteredPocket
    // ==========================================
    [HarmonyPatch(typeof(Plinko), "OnPuckEnteredPocket")]
    public static class PlinkoPocketPatch
    {
        static bool Prefix(Plinko __instance, ref int slotIndex)
        {
            if (!Plugin.rigPlinko) return true;

            var multipliersField = AccessTools.Field(typeof(Plinko), "slotMultipliers");
            var multipliers = multipliersField?.GetValue(__instance) as double[];

            if (multipliers != null && multipliers.Length > 0)
            {
                int maxIndex = 0;
                double maxVal = multipliers[0];

                for (int i = 1; i < multipliers.Length; i++)
                {
                    if (multipliers[i] > maxVal)
                    {
                        maxVal = multipliers[i];
                        maxIndex = i;
                    }
                }

                slotIndex = maxIndex;
            }

            return true;
        }
    }

    // ==========================================
    // ПАТЧ 10: Slots — GetWinningPatterns
    // ==========================================
    [HarmonyPatch(typeof(Slots), "GetWinningPatterns")]
    public static class SlotsJackpotPatch
    {
        static void Postfix(Slots __instance, ref object __result)
        {
            if (!Plugin.rigSlots) return;

            var patternsField = AccessTools.Field(typeof(Slots), "patterns");
            var patterns = patternsField?.GetValue(__instance) as List<SlotPattern>;
            if (patterns == null || patterns.Count == 0) return;

            Type winDataType = AccessTools.Inner(typeof(Slots), "SlotsWinData");
            Type listType = typeof(List<>).MakeGenericType(winDataType);
            var fakeWinList = (IList)Activator.CreateInstance(listType);

            float bestSymbolMultiplier = 1.75f;

            foreach (var pattern in patterns)
            {
                if (pattern != null)
                {
                    object winData = Activator.CreateInstance(winDataType, new object[] { bestSymbolMultiplier, pattern });
                    fakeWinList.Add(winData);
                }
            }

            __result = fakeWinList;
        }
    }
}