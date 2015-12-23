# NativeMessenger

UnityでのAndroidとiOSむけのシステムメッセージ表示用プラグイン  

* iOS8以上  
* Android4.0以上

## 導入

1. DownLoadして`Anz`フォルダを`Asset`のとこに配置  

終わり。

## 使い方

	// トースト表示
	Anz.Utils.NativeMessenger.ShowToast("メッセージ");

	// アラートダイアログ表示
	Anz.Utils.NativeMessenger.ShowAlert("メッセージ", () => {
		// ダイアログ閉じた
	});


## 詳細

### iOS

* `ShowToast()`

iOSにはToastないので、`UIView`と`UILabel`でToast風

* `ShowAlert()`

`UIAlertController`をつかってるので**iOS8以上**

### Android

* `ShowToast()`

`Toast`をつかってメッセージ表示

* `ShowAlert()`

`DialogFragment`をつかってダイアログ表示
