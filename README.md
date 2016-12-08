## Simple *Tic-Tac-Toe* game implementation using Unity3D game engine.

[WebGL build](https://troilk.github.io/TicTacToe/)

Game consists of 2 scenes (Menu and Gameplay).
In Menu scene you can select 1 of 3 difficulties.
* On Impossible AI will use MinMax tree to find optimal move.
* On Normal difficulty AI will use MinMax tree but with some probability it will select random move instead of optimal on each turn.
* On Easy difficulty you will play against random AI - it will select random free tile on map to place it’s mark.

In gameplay scene you can view your current stats (victories, losses, draws). NOTE: these counters are unique for each difficulty level.
Player whose turn is in progress is displayed in the bottom - for now AI decides it’s turn very fast (MinMax tree is generated and kept in memory), so you will not see enemy turn displayed.

Game is implemented using MVC and State Machine patterns.

### Used Libraries
* [GoKitLite](https://github.com/prime31/GoKitLite) - for easings
* [StateKit](https://github.com/prime31/StateKit) - lightweight state machine implementation
* [SoundKit](https://github.com/prime31/SoundKit) - simple pooled audio playback helper
* [NotNullAttribute](https://github.com/redbluegames/unity-notnullattribute) - custom attribute, used to ensure that object references are properly set up

### Used Images/Textures
* Some icons from [Font Awesome by Dave Gandy](http://fontawesome.io/). [License](http://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL)

### Used Fonts
* [Bungee](https://fonts.google.com/specimen/Bungee). [License](http://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL_web)

### Used Sounds/SFX
* [Powerup/success.wav](https://www.freesound.org/people/GabrielAraujo/sounds/242501/) by [GabrielAraujo](https://www.freesound.org/people/GabrielAraujo/). [License](https://creativecommons.org/licenses/by/3.0/)
* [Powerup/success.wav](https://www.freesound.org/people/GabrielAraujo/sounds/242501/) by [GabrielAraujo](https://www.freesound.org/people/GabrielAraujo/). [License](https://creativecommons.org/publicdomain/zero/1.0/)
* [Access Denied](https://www.freesound.org/people/suntemple/sounds/249300/) by [suntemple](https://www.freesound.org/people/suntemple/). [License](https://creativecommons.org/publicdomain/zero/1.0/)
* Glovebox Open from [http://www.freesfx.co.uk](http://www.freesfx.co.uk/)