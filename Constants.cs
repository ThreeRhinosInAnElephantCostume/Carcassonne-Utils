#if GODOT
#define NO_MIN_AI_DELAYS
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
    public static class DataPaths
    {
        public const string TILE_DIRECTORY = "res://Data/Tiles";
        public const string TILESET_DIRECTORY = "res://Data/Tilesets";

        public const string TILE_MODEL_DIRECTORY = "res://Models/Tiles";
        public const string PLAYER_THEMES_DIRECTORY = "res://Themes/PlayerThemes";
        public const string DEFAULT_PLAYER_AVATAR_PATH = "res://Icons/Themeable/PlayerAvatars/DefaultAvatar.png";
        public const string DEFAULT_PLAYER_ICON_PATH = "res://Icons/Themeable/PlayerIcons/DefaultIcon.png";
        public const string DEFAULT_PLAYER_THEME_PATH = PLAYER_THEMES_DIRECTORY + "/DefaultTheme.json";
        public const string THEMEABLE_ICONS_DIRECTORY = "res://Icons/Themeable";
        public const string SETTINGS_PATH = "user://settings.json"; 
        public const string STATISTICS_PATH = "user://Statistics";

        public const string SAVES_BASE_PATH = "user://Saves";
        public const string SAVES_LOCAL_PATH = SAVES_BASE_PATH + "/Local";
        public const string SAVES_MULTIPLAYER_PATH = SAVES_BASE_PATH + "/Multiplayer";

        public const string SAVE_ENGINE_FILE = "STATE";
        public const string SAVE_NAME_FILE = "NAME";
        public const string SAVE_RNG_FILE = "RNG";
        public const string SAVE_EXTRA_FILE = "GAME";
        public const string SAVE_AGENTS_SUBFOLDER = "AGENTS";

        public const string TEMPORATY_FILE_PATH = "user://Temp";


        public static List<string> REQUIRED_PATHS = new List<string>()
        {
            TILE_DIRECTORY,
            TILESET_DIRECTORY,
            TILE_MODEL_DIRECTORY,
            PLAYER_THEMES_DIRECTORY,
            THEMEABLE_ICONS_DIRECTORY,
            SAVES_BASE_PATH,
            SAVES_LOCAL_PATH,
            SAVES_MULTIPLAYER_PATH,
            STATISTICS_PATH,
        };
    }

    public static class AssetPaths
    {
        public const string BANNER_PROP_SHADER = "res://Props/PropBanner.gdshader";
        public const string SPRITE2D_PROP_SHADER = "res://Props/Sprite2DProp.gdshader";
    }
    public static class Colors
    {
        public static Godot.Color RoadColor { get; } = new Godot.Color(0.9f, 0.9f, 0.05f);
        public static Godot.Color FarmColor { get; } = new Godot.Color(0.3f, 0.95f, 0.3f);
        public static Godot.Color CityColor { get; } = new Godot.Color(0.4f, 0.4f, 0.1f);
        public static Godot.Color MonasteryColor { get; } = new Godot.Color(0.4f, 0.4f, 0f);
    }
    public static class ShaderParams
    {
        public static class ThemableShader
        {
            public const string SHADER_PRIMARY_THEME_SETTER = "primary_color";
            public const string SHADER_SECONDARY_THEME_SETTER = "secondary_color";
            public const string SHADER_TERTIARY_THEME_SETTER = "tertiary_color";
            public const string SHADER_PRIMARY_ENABLED = "primary_enabled";
            public const string SHADER_SECONDARY_ENABLED = "secondary_enabled";
            public const string SHADER_TERTIARY_ENABLED = "tertiary_enabled";
            public const string SHADER_ICON_ENABLED_THEME_SETTER = "icon_enabled";
            public const string SHADER_ICON_TRANSFORM = "icon_transform";
            public const string SHADER_ICON_CENTERED = "icon_centered";
            public const string SHADER_ICON_TEXTURE_THEME_SETTER = "icon_texture";
            public const string SHADER_ICON_SCALE_THEME_SETTER = "icon_scale";
            public const string SHADER_ICON_OFFSET_THEME_SETTER = "icon_offset";
            public const string SHADER_MASK_ENABLED_THEME_SETTER = "mask_enabled";
            public const string SHADER_MASK_TEXTURE_THEME_SETTER = "mask_texture";
            public const string SHADER_BILLBOARD_ENABLED = "billboard_enabled";
            public const string SHADER_TEXTURE_ENABLED = "texture_enabled";
            public const string SHADER_BACKGROUND_OPACITY = "background_alpha";
            public const string SHADER_BACKGROUND_COLOR = "background_color";
            public const string SHADER_SPRITE_ARRAY = "sprite_array";
            public const string SHADER_SPRITE_INDEX = "sprite_index";
        }
    }
    public const bool AUTO_SAVE_STATISTICS = true;
    #if NO_MIN_AI_DELAYS && DEBUG
    public const float MINIMUM_AI_DELAY_S = 0.0f;
    #else
    public const float MINIMUM_AI_DELAY_S = 0.5f;
    #endif
    public const float TILE_HEIGHT = 0.10f;
    public const float TILE_SIDE_LENGTH = 1.0f;
    public static readonly Godot.Vector2 TILE_SIZE = new Godot.Vector2(TILE_SIDE_LENGTH, TILE_SIDE_LENGTH);

}
#endif
