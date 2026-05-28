# Architecture Deprecations

## Removed

- `Assets/Project/Scripts/Systems/Player/PlayerController.cs`
  - Replaced by `PlayerInputHandler` + `PlayerStateController`.
  - No scene or prefab references were found by script GUID.

- `Assets/Project/Scripts/UI/Inventory/Test.cs`
  - Empty file.

## Deprecated But Still Referenced

- `Assets/Project/Scripts/Systems/Inventory/InventoryTester_Deprecated.cs`
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

## Renamed / Split In Phase 1

- `NotificacionData.cs` -> `NotificationData.cs`
- `HealthComponent .cs` -> `HealthComponent.cs`
- `ScannableObject .cs` -> `ScannableObject.cs`
- `SeparationDatabase.cs` -> `SeparationDatabase_SO.cs`
- `Deprecado_InventoryTester.cs` -> `InventoryTester_Deprecated.cs`
- `UIState` moved from `PlayerStateController.cs` to `UIState.cs`
- `PlayerSaveData` moved from `GameData.cs` to `PlayerSaveData.cs`
- `InventorySaveData` moved from `GameData.cs` to `InventorySaveData.cs`
- `SaveInfo` moved from `SaveController.cs` to `SaveInfo.cs`
- `NotificationType` moved from `NotificationData.cs` to `NotificationType.cs`

## Next Cleanup Pass

- Remove deprecated components from scenes/prefabs in the Unity Editor.
- After scene references are removed, delete:
  - `InventoryTester_Deprecated.cs`
  - `InteractionController.cs`
  - `InteractionUI.cs`
- Replace manual inventory tests with EditMode tests for `InventorySystem`.
