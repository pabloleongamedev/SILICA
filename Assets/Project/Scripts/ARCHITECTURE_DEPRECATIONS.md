# Architecture Deprecations

## Removed

- `Assets/Project/Scripts/Systems/Player/PlayerController.cs`
  - Replaced by `PlayerInputHandler` + `PlayerStateController`.
  - No scene or prefab references were found by script GUID.

- `Assets/Project/Scripts/UI/Inventory/Test.cs`
  - Empty file.

## Deprecated But Still Referenced

- `Assets/Project/Scripts/Systems/Inventory/InventoryTester.cs`
  - Manual keyboard-driven debug harness.
  - Referenced by active/test scenes.
  - Keep only in test scenes until replaced by automated domain tests.

- `Assets/Project/Scripts/Systems/Player/Interaction/InteractionController.cs`
  - Duplicate interaction input path.
  - Use `PlayerInputHandler` as the single input entry point.
  - Still referenced in some scenes/prefabs.

- `Assets/Project/Scripts/UI/Interact/InteractionUI.cs`
  - Duplicate interaction prompt UI.
  - Use `InteractionUIController`.
  - Still referenced by `Assets/Project/Prefabs/Canvas.prefab`.

## Repaired

- `Assets/Project/Scripts/Core/GameManager.cs`
  - Reactivated as a real singleton save orchestrator.

- `Assets/Project/Scripts/Core/GameRestorer.cs`
  - Reactivated to restore player transform and inventory.

- `Assets/Project/Scripts/UI/Menu/MainMenuManager.cs`
  - Replaced commented legacy content with an active `MainMenuManager` class.

## Next Cleanup Pass

- Remove deprecated components from scenes/prefabs in the Unity Editor.
- After scene references are removed, delete:
  - `InventoryTester.cs`
  - `InteractionController.cs`
  - `InteractionUI.cs`
- Replace manual inventory tests with EditMode tests for `InventorySystem`.
