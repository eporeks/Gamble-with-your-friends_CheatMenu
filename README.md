# EPOREKS // PRIV — In-Game Menu for Gamble With Your Friends

<p align="center">
  <img width="539" height="423" alt="image" src="https://github.com/user-attachments/assets/b457c06f-0bdf-448f-99fe-22766ef97848" />
  <img width="539" height="419" alt="image" src="https://github.com/user-attachments/assets/5a866d74-9592-4fb8-914d-1e9177ab6123" />
  <img width="541" height="418" alt="image" src="https://github.com/user-attachments/assets/043ef90c-791a-4424-8224-ba4eb34e47e0" />
</p>

## Возможности

### Crash & Arcade
* **Crash — Infinite Multiplier:** фиксирует точку краша на `9,999,999x`.
* **CrossyRoad — God Mode:** обнуляет вероятность падения пингвина на каждом шаге.
* **Dragon Tower — Safe Path:** убирает мины (яйца) со всех этажей башни с корректным начислением выплаты.
* **Minesweeper — No Mines:** очищает сетку мин при старте раунда с сохранением выбранного множителя.
* **Plinko — Max Slot Magnet:** направляет выплату в слот с максимальным коэффициентом (`5x`).

### Table, Wheels & Slots
* **Blackjack — Instant 21:** гарантирует выдачу Туза и 10-очковой карты игроку при старте раздачи.
* **Roulette — Zero Rig:** фиксирует выпадение сектора `0`.
* **MoneyWheel — 10x Orange:** автоматически ставит на `Orange` и фиксирует выигрыш `10x`.
* **Slots — Force Jackpot:** активирует все выигрышные линии с максимальным символом (`Jackpot!`).
* **Duck Race — Turbo Win:** ускоряет выбранную утку и замедляет соперников.

## Установка и использование
1. Скачайте и установите [Release](https://github.com/eporeks/casino-bet/releases/tag/release)
2. Перейдите в папку игры и перекиньте туда все файлы из этой папки
   
ИЛИ

1. Скачайте и установите **[BepInEx 5 (x64)](https://github.com/BepInEx/BepInEx/releases)** в корневую папку с игрой.
2. Скопируйте скомпилированный `.dll` файл плагина в папку `BepInEx/plugins/`.
3. Запустите игру и создайте лобби (все модули работают на стороне хоста `NetworkServer.active`).
4. Нажмите **`INSERT`**, чтобы скрыть или открыть меню.

## Стек
* C# / .NET Framework 4.x
* [BepInEx 5](https://github.com/BepInEx/BepInEx) & HarmonyX
* Unity IMGUI & Mirror Networking

## Связь со мной

Discord: wyx2
