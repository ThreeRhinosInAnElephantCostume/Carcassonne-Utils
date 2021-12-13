﻿#if GODOT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading;

using static System.Math;

using static Utils;

public static class Constants
{
    public const string TILE_DIRECTORY = "res://Data/Tiles";
    public const string TILESET_DIRECTORY = "res://Data/Tilesets";

    public const string TILE_MODEL_DIRECTORY = "res://Models/Tiles";
    public const string PLAYER_THEMES_DIRECTORY = "res://Themes/PlayerThemes";
    public const string THEMEABLE_ICONS_PATH = "res://Icons/Themeable";

    public const string SETTINGS_PATH = "user://settings.json"; 

    public const float TILE_HEIGHT = 0.10f;
    public const float TILE_SIDE_LENGTH = 1.0f;
    public static readonly Godot.Vector2 TILE_SIZE = new Godot.Vector2(TILE_SIDE_LENGTH, TILE_SIDE_LENGTH);

}
#endif
