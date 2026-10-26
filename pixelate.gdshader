shader_type canvas_item;

uniform sampler2D screen_tex : hint_screen_texture, filter_nearest;
uniform float pixel_size : hint_range(1.0, 16.0) = 4.0;

void fragment() {
    vec2 res = 1.0 / SCREEN_PIXEL_SIZE;
    vec2 block = floor(FRAGCOORD.xy / pixel_size) * pixel_size + pixel_size * 0.5;
    COLOR = texture(screen_tex, block / res);
    COLOR.rgb = floor(COLOR.rgb * 6.0 + 0.5) / 6.0;
}