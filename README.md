https://gajumaru314159.github.io/DependencyViewer/

# UnrealEngineのモジュール依存関係の確認方法
```XXX.Build.cs```のコンストラクタ内でリフレクションを使用してモジュールの依存関係を抽出することができます。```.uproject```を右クリックして```Generate Visual Studio project files```を実行するとUnrealBuildToolを通してコードが実行されます。

実装例：
```ModuleCheck.Build.cs```
