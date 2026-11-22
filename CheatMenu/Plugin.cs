using BepInEx;
using HarmonyLib;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestMenu
{
    [BepInPlugin("com.cheat.menu", "InGame Menu", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private bool show = true;
        // Увеличили высоту окна под новые кнопки
        private Rect rect = new Rect(30, 30, 300, 380);

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

        // Стили и текстуры для UI
        private bool stylesInitialized = false;
        private GUIStyle windowStyle;
        private GUIStyle headerStyle;
        private GUIStyle subHeaderStyle;
        private GUIStyle cardStyle;
        private GUIStyle buttonStyle;
        private GUIStyle toggleBgOnStyle;
        private GUIStyle toggleBgOffStyle;
        private GUIStyle labelStyle;
        private GUIStyle footerStyle;

        private Texture2D accentTex;
        private Texture2D dividerTex;

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
            if (stylesInitialized) return;

            Color bgDark = new Color(0.07f, 0.07f, 0.10f, 0.96f);
            Color cardBg = new Color(0.11f, 0.12f, 0.16f, 1f);
            Color btnNormal = new Color(0.15f, 0.17f, 0.23f, 1f);
            Color btnHover = new Color(0.20f, 0.24f, 0.33f, 1f);
            Color btnActive = new Color(0.00f, 0.70f, 0.85f, 1f);
            Color accentCyan = new Color(0.00f, 0.85f, 1.00f, 1f);
            Color onGreen = new Color(0.10f, 0.78f, 0.48f, 1f);
            Color offGray = new Color(0.22f, 0.23f, 0.28f, 1f);
            Color borderCol = new Color(0.22f, 0.25f, 0.34f, 0.8f);

            accentTex = MakeTex(2, 2, accentCyan);
            dividerTex = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.08f));

            windowStyle = new GUIStyle(GUI.skin.window);
            windowStyle.normal.background = MakeBorderedTex(64, 64, bgDark, borderCol);
            windowStyle.onNormal.background = windowStyle.normal.background;
            windowStyle.padding = new RectOffset(14, 14, 14, 12);
            windowStyle.border = new RectOffset(2, 2, 2, 2);

            headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.fontSize = 16;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;
            headerStyle.alignment = TextAnchor.MiddleLeft;

            subHeaderStyle = new GUIStyle(GUI.skin.label);
            subHeaderStyle.fontSize = 10;
            subHeaderStyle.fontStyle = FontStyle.Bold;
            subHeaderStyle.normal.textColor = accentCyan;
            subHeaderStyle.alignment = TextAnchor.MiddleRight;

            cardStyle = new GUIStyle(GUI.skin.box);
            cardStyle.normal.background = MakeBorderedTex(32, 32, cardBg, new Color(1f, 1f, 1f, 0.05f));
            cardStyle.padding = new RectOffset(10, 10, 8, 8);
            cardStyle.margin = new RectOffset(0, 0, 4, 4);

            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 13;
            labelStyle.normal.textColor = new Color(0.88f, 0.90f, 0.95f, 1f);
            labelStyle.alignment = TextAnchor.MiddleLeft;

            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 13;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = accentCyan;
            buttonStyle.active.textColor = Color.black;
            buttonStyle.normal.background = MakeBorderedTex(32, 32, btnNormal, borderCol);
            buttonStyle.hover.background = MakeBorderedTex(32, 32, btnHover, accentCyan);
            buttonStyle.active.background = MakeTex(2, 2, btnActive);
            buttonStyle.padding = new RectOffset(10, 10, 9, 9);
            buttonStyle.margin = new RectOffset(0, 0, 4, 4);

            toggleBgOnStyle = new GUIStyle(GUI.skin.button);
            toggleBgOnStyle.fontSize = 11;
            toggleBgOnStyle.fontStyle = FontStyle.Bold;
            toggleBgOnStyle.normal.textColor = Color.black;
            toggleBgOnStyle.hover.textColor = Color.black;
            toggleBgOnStyle.normal.background = MakeTex(2, 2, onGreen);
            toggleBgOnStyle.hover.background = MakeTex(2, 2, new Color(0.15f, 0.88f, 0.55f, 1f));
            toggleBgOnStyle.fixedWidth = 52;
            toggleBgOnStyle.fixedHeight = 24;

            toggleBgOffStyle = new GUIStyle(GUI.skin.button);
            toggleBgOffStyle.fontSize = 11;
            toggleBgOffStyle.fontStyle = FontStyle.Bold;
            toggleBgOffStyle.normal.textColor = new Color(0.65f, 0.68f, 0.75f, 1f);
            toggleBgOffStyle.hover.textColor = Color.white;
            toggleBgOffStyle.normal.background = MakeTex(2, 2, offGray);
            toggleBgOffStyle.hover.background = MakeTex(2, 2, new Color(0.28f, 0.30f, 0.38f, 1f));
            toggleBgOffStyle.fixedWidth = 52;
            toggleBgOffStyle.fixedHeight = 24;

            footerStyle = new GUIStyle(GUI.skin.label);
            footerStyle.fontSize = 10;
            footerStyle.normal.textColor = new Color(0.45f, 0.48f, 0.56f, 1f);
            footerStyle.alignment = TextAnchor.MiddleCenter;

            stylesInitialized = true;
        }

        private void OnGUI()
        {
            if (!show) return;

            InitStyles();
            rect = GUI.Window(0, rect, WindowFunc, "", windowStyle);
        }

        private void WindowFunc(int id)
        {
            GUI.DrawTexture(new Rect(0, 0, rect.width, 3), accentTex);

            GUILayout.BeginHorizontal();
            GUILayout.Label("CASINO PANEL", headerStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label("v1.1", subHeaderStyle);
            GUILayout.EndHorizontal();

            GUILayout.Space(6);
            DrawDivider();
            GUILayout.Space(6);

            // Вспомогательный метод для отрисовки переключателей
            DrawToggleCard("100% победа в утках", ref rigDuckRace);
            DrawToggleCard("Пингвин прыгает до конца", ref zeroCrashChance);
            DrawToggleCard("Краш до 999999x", ref infiniteCrashPoint);
            DrawToggleCard("Blackjack постоянно 21", ref rigBlackjack);
            DrawToggleCard("Башня дракона без мин", ref customDragonTower);
            DrawToggleCard("Только 0 в рулетке", ref alwaysZeroRoulette);
            DrawToggleCard("Мины без мин", ref customMinesweeper);
            DrawToggleCard("MoneyWheel 100% 10x", ref rigMoneyWheel);
            DrawToggleCard("Plinko 100% 5x", ref rigPlinko);
            DrawToggleCard("Слоты 100% Джекпот", ref rigSlots);

            GUILayout.FlexibleSpace();

            DrawDivider();
            GUILayout.Space(4);
            GUILayout.Label("[ INSERT ] — скрыть / открыть меню", footerStyle);

            GUI.DragWindow(new Rect(0, 0, rect.width, 40));
        }

        private void DrawToggleCard(string label, ref bool state)
        {
            GUILayout.BeginVertical(cardStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, labelStyle);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(state ? "ON" : "OFF", state ? toggleBgOnStyle : toggleBgOffStyle))
            {
                state = !state;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
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
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private Texture2D MakeBorderedTex(int width, int height, Color fill, Color border)
        {
            Texture2D result = new Texture2D(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isBorder = (x == 0 || y == 0 || x == width - 1 || y == height - 1);
                    result.SetPixel(x, y, isBorder ? border : fill);
                }
            }
            result.Apply();
            return result;
        }
    }

    // ==========================================
    // ПАТЧ 1: GetCrashChanceForStep
    // Замени CrashGame на реальное имя класса!
    // ==========================================
    [HarmonyPatch(typeof(CrossyRoad), "GetCrashChanceForStep")]
    public static class CrashChancePatch
    {
        static bool Prefix(ref float __result)
        {
            if (!Plugin.zeroCrashChance) return true;

            __result = 0f;
            return false; // Пропускаем оригинальный метод
        }
    }

    // ==========================================
    // ПАТЧ 2: SetCrashPoint
    // Замени CrashGame на реальное имя класса!
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
    // ПАТЧ 3: Blackjack DealCardToPlayer
    // ==========================================
    [HarmonyPatch(typeof(Blackjack), "DealCardToPlayer")]
    public static class BlackjackDealPatch
    {
        static bool Prefix(Blackjack __instance, bool isFaceDown)
        {
            if (!Plugin.rigBlackjack) return true;

            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Void Blackjack::DealCardToPlayer(System.Boolean)' called when server was not active");
                return false;
            }

            // Получаем доступ к приватным полям и методам класса Blackjack
            var deckField = AccessTools.Field(typeof(Blackjack), "deck");
            var playerHandField = AccessTools.Field(typeof(Blackjack), "playerHand");
            var spawnedPlayerCardsField = AccessTools.Field(typeof(Blackjack), "spawnedPlayerCards");

            var initDeckMethod = AccessTools.Method(typeof(Blackjack), "InitializeDeck");
            var dealToHandMethod = AccessTools.Method(typeof(Blackjack), "DealCardToHand");

            var deck = (List<CardData>)deckField.GetValue(__instance);
            var playerHand = (List<CardData>)playerHandField.GetValue(__instance);
            var spawnedPlayerCards = spawnedPlayerCardsField.GetValue(__instance);

            if (deck.Count == 0)
            {
                initDeckMethod.Invoke(__instance, null);
                deck = (List<CardData>)deckField.GetValue(__instance);
            }

            int num = -1;
            if (playerHand.Count == 0)
            {
                num = deck.FindIndex((CardData c) => c.Rank == Rank.Ace);
            }
            else if (playerHand.Count == 1)
            {
                num = deck.FindIndex((CardData c) => c.GetBlackjackValue() == 10);
            }

            if (num > 0)
            {
                CardData item = deck[num];
                deck.RemoveAt(num);
                deck.Insert(0, item);
            }

            // Получаем enum Blackjack.CardAreaType.Player
            Type cardAreaEnum = AccessTools.Inner(typeof(Blackjack), "CardAreaType");
            object playerAreaVal = Enum.Parse(cardAreaEnum, "Player");

            dealToHandMethod.Invoke(__instance, new object[] { playerHand, spawnedPlayerCards, playerAreaVal, isFaceDown });

            return false; // Пропускаем оригинальный метод
        }
    }

    // ==========================================
    // ПАТЧ 4: Твой оригинальный DuckRace
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

                    // Вызываем ServerSetButtonState только если eggIndex валидный
                    if (buttons != null && eggIndex >= 0 && eggIndex < buttons.Count)
                    {
                        var btn = buttons[eggIndex] as DragonTowerButton;
                        btn?.ServerSetButtonState(DragonTowerButton.ButtonState.RevealEgg);
                    }
                }
            }

            return false; // Пропускаем оригинальный RevealEggs(), чтобы не словить IndexOutOfRange
        }
    }



    // Кастомный Random, который возвращает заданное число вместо случайного
    public class FixedRandom : System.Random
        {
            private readonly int _fixedValue;

            public FixedRandom(int fixedValue)
            {
                _fixedValue = fixedValue;
            }

            public override int Next(int minValue, int maxValue)
            {
                // Защита от выхода за пределы диапазона
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
            // Если флаг в плагине выключен — запускаем оригинальный метод
            if (!Plugin.alwaysZeroRoulette) return true;

            if (!NetworkServer.active) return false;

            // 1. Вызываем базовый метод GameBase.StartGame()
            BaseStartGameReversePatch.CallBaseStartGame(__instance);

            // 2. Получаем приватное поле wheel
            var wheelField = AccessTools.Field(typeof(Roulette), "wheel");
            var wheel = wheelField?.GetValue(__instance) as Wheel;

            if (wheel != null)
            {
                // Передаем FixedRandom(0), чтобы оба вызова rng.Next(0, 37) вернули 0
                wheel.SpinTheWheel(new FixedRandom(0));
            }

            return false; // Пропускаем оригинальный Roulette.StartGame()
        }
    }

    [HarmonyPatch(typeof(GameBase), "StartGame")]
    public static class BaseStartGameReversePatch
    {
        [HarmonyReversePatch]
        public static void CallBaseStartGame(GameBase instance) => throw new NotImplementedException();
    }

    [HarmonyPatch(typeof(Minesweeper), "SetMines")]
    public static class SetMinesPatch
    {
        static bool Prefix(Minesweeper __instance)
        {
            if (!Plugin.customMinesweeper) return true;
            if (!NetworkServer.active) return false;

            var mineIndexesField = AccessTools.Field(typeof(Minesweeper), "_mineIndexes");
            var mineIndexes = mineIndexesField?.GetValue(__instance) as HashSet<int>;

            // Очищаем список индексов мин — ни одна клетка не взорвётся
            mineIndexes?.Clear();

            return false; // Пропускаем оригинальную расстановку мин
        }
    }

    [HarmonyPatch(typeof(MoneyWheel), "StartGame")]
    public static class MoneyWheelStartGamePatch
    {
        static bool Prefix(MoneyWheel __instance)
        {
            if (!Plugin.rigMoneyWheel) return true;
            if (!NetworkServer.active) return false;

            // Вызываем базовый GameBase.StartGame() (у вас уже есть BaseStartGameReversePatch)
            BaseStartGameReversePatch.CallBaseStartGame(__instance);

            // Автоматически выбираем ставку "Orange"
            __instance.SelectBettingOption("Orange");

            var wheelField = AccessTools.Field(typeof(MoneyWheel), "wheel");
            var wheel = wheelField?.GetValue(__instance) as Wheel;

            if (wheel != null)
            {
                // Подставьте сюда индекс сектора "Orange" из класса Wheel
                int orangeIndex = 0;
                wheel.SpinTheWheel(new FixedRandom(orangeIndex));
            }

            return false;
        }
    }

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

            return true; // Продолжаем выполнение оригинального OnPuckEnteredPocket с максимальным slotIndex
        }
    }

    [HarmonyPatch(typeof(Slots), "GetWinningPatterns")]
    public static class SlotsJackpotPatch
    {
        static void Postfix(Slots __instance, ref object __result)
        {
            if (!Plugin.rigSlots) return;

            var patternsField = AccessTools.Field(typeof(Slots), "patterns");
            var patterns = patternsField?.GetValue(__instance) as List<SlotPattern>;
            if (patterns == null || patterns.Count == 0) return;

            // Получаем приватный вложенный класс Slots.SlotsWinData
            Type winDataType = AccessTools.Inner(typeof(Slots), "SlotsWinData");
            Type listType = typeof(List<>).MakeGenericType(winDataType);
            var fakeWinList = (IList)Activator.CreateInstance(listType);

            // В оригинале формула: 1f - ((float)num - 1.5f) * 0.5f.
            // При num = 0 (самый редкий/дорогой символ) множитель равен 1.75f.
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