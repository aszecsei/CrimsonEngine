# CrimsonEngine
A framework extending MonoGame to make it similar to Unity 2D

## Instructions
To use this project, add it as a subproject to your main MonoGame project.

Inside Game1.cs:
```C#
protected override void Initialize()
{
  // TODO: Add your initialization logic here
  SceneCreator.GraphicsDevice = GraphicsDevice;
  SceneManager.CurrentScene = SceneCreator.GameScene();
  base.Initialize();
}

protected override void Update(GameTime gameTime)
  {
  if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
    Exit();

  SceneManager.Update(gameTime);

  base.Update(gameTime);
}

protected override void Draw(GameTime gameTime)
{
  SceneManager.Draw(GraphicsDevice, spriteBatch, gameTime);

  base.Draw(gameTime);
}
```

Create a ```SceneCreator``` class:

```C#
public static GraphicsDevice GraphicsDevice;

public static Scene GameScene()
{
  Scene mainScene = new Scene();
  SceneBase(mainScene);
  mainScene.backgroundColor = Color.Black;

  // Insert scene creation here

  return mainScene;
}

private static void SceneBase(Scene scene)
{
  scene.CurrentCamera = new Camera2D();
  scene.CurrentCamera.ViewportHeight = GraphicsDevice.Viewport.Height;
  scene.CurrentCamera.ViewportWidth = GraphicsDevice.Viewport.Width;
}
```

Subclass the ```Component``` class to add update functionality, or the ```Renderer``` class to add drawing functionality.
