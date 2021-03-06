# COM3D2.CustomResolutionScreenShot.Plugin (CRSS)
内部解像度を変更してスクリーンショットを撮影できるプラグインです  
~~解像度は、力だ~~
  
## ダウンロード
[こちら](https://github.com/AshleyScarlet/COM3D2.CustomResolutionScreenShot.Plugin/releases/latest/download/COM3D2.CustomResolutionScreenShot.Plugin.zip)からどうぞ  
COM3D2 ver1.55で使うことを想定していますが多分どのバージョンでも動きそうな気がします


### `COM3D2.Hi-ResSS.Plugin` をお持ちの方へ
このプラグインはHi-ResSSのリメイク版です  
~~機能が同じなのに同じものが二つあるのは気持ちが悪いので~~  このプラグイン導入する際はHi-ResSSプラグインを削除することをお勧めします  
<sub><sub>実際は余計な機能を消したので同じではないです</sub></sub>

## 機能
導入すると歯車メニューにアイコンが３つ追加されます
<img src="https://user-images.githubusercontent.com/70315656/116590856-30bfed00-a959-11eb-8eeb-5b1eb3ee78fd.png" width="25" height="25">
<img src="https://user-images.githubusercontent.com/70315656/116591208-87c5c200-a959-11eb-864c-65a2e44d6354.png" width="25" height="25">
<img src="https://user-images.githubusercontent.com/70315656/116764003-77e1d700-aa5a-11eb-915c-a58b1f08895a.png" width="25" height="25">
___
### ![](https://user-images.githubusercontent.com/70315656/116590856-30bfed00-a959-11eb-8eeb-5b1eb3ee78fd.png)
プリセットで指定した解像度で撮影します  
内部解像度を変更するのでキャプチャ倍率を使用するよりキレイな高解像度SSが撮れたりします
| CRSS  (3840x2160) | キャプチャ倍率  (1920x1080 X2) |
|:---:|:---:|
|<img src="https://user-images.githubusercontent.com/70315656/116593066-a927ad80-a95b-11eb-8d98-73834d959133.png">|<img src="https://user-images.githubusercontent.com/70315656/116593087-af1d8e80-a95b-11eb-942c-10e3747ed28b.png">|  

~~わからん~~
___
### ![](https://user-images.githubusercontent.com/70315656/116591208-87c5c200-a959-11eb-864c-65a2e44d6354.png)
カスタム解像度 + 透過スクリーンショット  
キャラクターだけを撮影します  
| 通常 | 高品質ON |
|:---:|:---:|
|<img src="https://user-images.githubusercontent.com/70315656/116764042-9cd64a00-aa5a-11eb-89b8-6241bca9fcb9.png">|<img src="https://user-images.githubusercontent.com/70315656/116764034-9a73f000-aa5a-11eb-8718-e5e47a852133.png">|
| 若干キャラの周りに影が出ます | きれいだけど撮影に時間がかかり気味 |  

こんなかんじ
___
### ![](https://user-images.githubusercontent.com/70315656/116764003-77e1d700-aa5a-11eb-915c-a58b1f08895a.png)
みすぼらしいGUIを開きます  
![](https://user-images.githubusercontent.com/70315656/116764120-e32ba900-aa5a-11eb-9fe8-222d77cb97f7.png)  
- プリセット
  - 設定ファイル内のプリセットを選べます
- 高品質モード
  - 透過撮影で高品質モードを使うかどうか
- プレビュー表示
  - 画面左下にプレビュー画面を表示します 

#### プレビュー
![](https://user-images.githubusercontent.com/70315656/116764196-49183080-aa5b-11eb-88a6-6e84cf36e2ca.png)  
↑この部分  
縦向きやワイドなSSを撮影する際にひょっとしたら使えるかもしれません...  

## 設定ファイル
`Sybaris\UnityInjector\Config\CustomResolutionScreenShot.xml` が設定ファイルです  
初期状態の中身です
```xml
<?xml version="1.0" encoding="UTF-8"?>
<Config>
	<!-- 解像度プリセット Targetに使用するプリセットの名前を入力してください -->
	<Presets Target="4K">
		
		<!-- フルHDのプリセット フルスクリーン状態で撮影するのと変わらないです -->
		<Preset Name="FullHD">
			<Width>1920</Width>
			<Height>1080</Height>
		</Preset>

		<!-- 4Kのプリセット 良い感じ -->
		<Preset Name="4K">
			<Width>3840</Width>
			<Height>2160</Height>
		</Preset>

		<!-- UWQHDのプリセット 横に少し長い -->
		<Preset Name="UWQHD">
			<Width>3440</Width>
			<Height>1440</Height>
		</Preset>

		<!-- 8Kのプリセット 結構重たいので注意 -->
		<Preset Name="8K">
			<Width>7680</Width>
			<Height>4320</Height>
		</Preset>

		<!-- 16Kのプリセットですが確実に描画が崩れてしまう... -->
		<Preset Name="16K">
			<Width>15360</Width>
			<Height>8640</Height>
		</Preset>
		
	</Presets>
</Config>
```
8Kのような大きい解像度で撮影する際は結構なマシンスペックを要求されます

## 問題点
### SceneCaptureのエフェクトと相性が悪い
主に背景ぼかしやブルーム系のエフェクト  
解像度を上げたりすると画面とSSとでかなり違いが出てしまいます

| CRSS  (3840x2160) | ふつうに撮ったやつ  (画面そのまま) |
|:---:|:---:|
|<img src="https://user-images.githubusercontent.com/70315656/116599090-96fd3d80-a962-11eb-8bdf-76ec675f1893.png">|<img src="https://user-images.githubusercontent.com/70315656/116599114-9cf31e80-a962-11eb-9112-f5d4695dd68d.png">|
| Bokehのみ、両方おなじ設定 |


 ~~SceneCaptureを改造すれば何とかできます~~  
`OnRenderImage(... , ...)` 内でRenderTextureのサイズを取得しているところをScreenのサイズを取得するようにすればいい感じになるはずです（憶測） 

また、一部のエフェクトを適用していると透過撮影が出来なくなる可能性アリ。  
~~不具合まみれ~~

### 大きい解像度を指定すると撮影時に数秒固まってしまう
<sub>非同期にするべきなんですかね...?</sub>
