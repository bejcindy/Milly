# Liquid

## Material Parameters

### Basic

`Liveness` 控制点亮的动画
`Liquid Height` 液面高度，0-1
`Liquid Surface Color` 表面颜色（实际上是模型反面的颜色）

---

### Gradient

`Color_Top` `Color_Bot` 渐变的两个颜色
`HeightRemap` 调整渐变长度在世界坐标中的高度映射，前两位固定为 0、1，只需要调整后两位或保持不变
`GradientScale` 渐变的跨度
`GradientOffset` 渐变的偏移
`GradientPower` 渐变的突变程度

### Wave

`WaveSpeed` `WaveIntensity` `WaveAmplitude` 控制表面的波动

## Script

## Wobble

`Max Wobble` `Wobble Speed` `Recovery` 控制 Play Mode 移动液体时表面的晃动 (需要将 Material 拖到 Mat 中)

## Particle Effect (Optional)

`Liquid Effect` 当点亮时播放的粒子，比如气泡： `Assets/Aris/_ArtAsset/Prefabs/Liquid/Bubbles`
