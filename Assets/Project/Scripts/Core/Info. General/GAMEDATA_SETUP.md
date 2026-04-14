# 🎮 Sistema de Guardado Automático - Guía de Implementación

## ✅ Archivos Creados

### Core System (Scripts/Core/)
1. **GameData.cs** - Estructura serializable de todos los datos de la partida
2. **GameManager.cs** - Singleton que orquesta guardado/carga con auto-save
3. **SaveController.cs** - Manejo de I/O en disco con JsonUtility
4. **GameRestorer.cs** - Restaura estado del jugador al cargar una partida

### UI Actualizado (Scripts/UI/)
1. **SaveSlot.cs** - Integrado con GameManager (detecta guardos + actualiza visual)
2. **MainMenuManager.cs** - Sincroniza con GameManager

### Player Updated (Scripts/Systems/Player/)
1. **PlayerController.cs** - Sincroniza posición/rotación para auto-save

---

## 🔧 Pasos de Configuración en Unity

### Paso 1: Crear GameManager en la Escena del Menú
```
1. En MainMenu scene:
   > Create Empty GameObject
   > Rename to "GameManager"
   > Add Component > GameManager.cs
   > Configurar AutoSaveInterval = 60 (segundos)
```

### Paso 2: Configurar SaveSlots en el Panel de Juego
```
1. Seleccionar cada Button de SaveSlot
2. En el Inspector:
   > Add Component > SaveSlot.cs
   > Slot ID = 1, 2, 3 (según corresponda)
   > Drag TextLabel al campo
   > Opcional: Drag infoLabel al campo
3. En el Button:
   > Event > On Click()
   > Drag SaveSlot component
   > Dropdown > SaveSlot > OnSlotPressed()
```

### Paso 3: Crear GameRestorer en la Escena de Juego
```
1. En la escena del juego (GameScene):
   > Create Empty GameObject
   > Rename to "_GameRestorer"
   > Add Component > GameRestorer.cs
   > Ubicar al inicio de la escena (se ejecuta en Awake)
```

### Paso 4: Verificar Configuración de MainMenuManager
```
Ya configurado, pero verificar:
- mainPanel, playPanel, optionsPanel, creditsPanel asignados
- mainFirstButton, playFirstButton, etc. asignados
- Los botones del menú llamar: ShowPlayMenu(), ShowMainMenu(), etc.
```

---

## 🔄 Flujo de Funcionamiento

### 1️⃣ Al Abrir el Juego
```
Menú Principal
↓
Usuario presiona "Jugar"
↓
MainMenuManager.ShowPlayMenu()
  → GameManager.RefreshSaveStates()
  → SaveSlot[].RefreshSlot()
    ← Detecta archivos JSON en persistentDataPath
    ← Muestra "Continuar" o "Nueva Partida"
```

### 2️⃣ Usuario Selecciona Slot
```
Usuario presiona SaveSlot 1
↓
SaveSlot.OnSlotPressed()
  ↙ Si tiene datos             ↘ Si no tiene datos
  GameManager.LoadGame()       GameManager.CreateNewGame()
  ↓                            ↓
  LoadScene(savedScene)        LoadScene("GameScene")
  ↓                            ↓
  GameRestorer.Awake()         (PlayerController inicia normal)
  ↓
  Restaurar Posición/Rotación
  ↓
  Juego Continúa
```

### 3️⃣ Durante el Juego - Auto-Save
```
Cada Evento (UPDATE: cada 0.5s se sincroniza posición)
↓
GameManager.Update()
↓
if (timeSinceLastSave >= autoSaveInterval) → cada 60s
↓
AutoSave()
  → UpdatePlayerData() (sincronizar posición actual)
  → SaveController.SaveGame() (escribir JSON a disco)
  → Debug.Log("AUTO-SAVE ejecutado")
```

### 4️⃣ Al Salir del Juego
```
Usuario presiona Salir/Volver al Menú
↓
(Opcional) GameManager.SaveGame() antes de cargar menú
↓
SceneManager.LoadScene("MainMenu")
↓
MainMenuManager.ShowMainMenu()
↓
Esperar a que usuario vuelva a "Jugar"
```

---

## 📍 Rutas de Guardado

Los archivos se guardan en:
```
- Windows: C:\Users\[Usuario]\AppData\LocalLow\[Empresa]\[Juego]\Saves\
- macOS: /Library/Application Support/[Empresa]/[Juego]/Saves/
- Linux: ~/.config/unity3d/[Empresa]/[Juego]/Saves/

Estructura:
Saves/
├── save_1.json
├── save_2.json
└── save_3.json
```

---

## 🎯 Datos que se Guardan (GameData)

### Automáticamente Guardado:
- ✅ Posición del jugador (X, Y, Z)
- ✅ Rotación del jugador (X, Y, Z, W)
- ✅ Tiempo de juego (horas, minutos)
- ✅ Escena actual
- ✅ Timestamp del guardado
- ✅ ID del slot

### Métodos para Agregar Datos (desde otros scripts):
```csharp
// En cualquier momento del juego, los scripts pueden agregar data:
GameManager.Instance.UpdatePlayerHealth(health, maxHealth);
GameManager.Instance.AddInventoryItem(itemID, gridX, gridY, quantity);
GameManager.Instance.RegisterScannedElement(elementID);

// Estos datos se incluyen en el auto-save automáticamente
```

---

## 🐛 Debugging

### Ver Estado Actual de la Partida
```csharp
// En la Consola de Unity, pegar en Awake/Start:
Debug.Log(GameManager.Instance.DebugGetGameState());
```

Output:
```
=== GAME STATE ===
Slot: 1
Escena: GameScene
Tiempo Jugado: 1h 23m
Guardado: 2026-04-14 15:30:45
Items: 5
Elementos Escaneados: 12
Posición Jugador: (10.5, 2.0, -5.3)
==================
```

### Verificar Archivos Guardados
```
Windows: Abrir Explorer
%APPDATA%\..\..\LocalLow\[Empresa]\[Juego]\Saves\
```

---

## ⚙️ Configuración del Auto-Save (GameManager Inspector)

| Campo | Valor Recomendado | Descripción |
|-------|------------------|-------------|
| Auto Save Interval | 60 | Guardar cada 60 segundos |
| Enable Auto Save | ✓ | Activar auto-save |

### Para Testing:
Cambiar a `10` segundos para ver guardos frecuentes.

---

## 🚀 Jerarquía de Escenas Recomendada

```
📁 Scenes/
├── MainMenu (Contiene: MainMenuManager, GameManager, Canvas)
└── GameScene (Contiene: PlayerController, _GameRestorer, Mundo)

Flujo:
MainMenu → Cargar GameScene → Auto-save periódico → 
Volver a MainMenu → Ver "Continuar" disponible
```

---

## 🔗 Integración con Otros Sistemas

### Para InventorySystem:
```csharp
// En InventorySystem cuando se agrega un item:
GameManager.Instance.AddInventoryItem(
    itemData.itemID, 
    gridX, gridY, 
    quantity
);
```

### Para ScanSystem:
```csharp
// En ScanSystem cuando se escanea un elemento:
GameManager.Instance.RegisterScannedElement(element.ID);
```

### Para Salud del Jugador:
```csharp
// En PlayerHealth o similar:
GameManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
```

---

## ✨ Características

- ✅ Auto-save sin intervención del usuario
- ✅ Botón "Continuar" aparece solo si hay guardado
- ✅ Botón "Nueva Partida" siempre disponible
- ✅ Restauración automática de posición/rotación
- ✅ Múltiples slots (1, 2, 3)
- ✅ Serialización JSON nativa (sin dependencias)
- ✅ Acceso global: `GameManager.Instance`
- ✅ Arquitectura escalable para nuevos datos
- ✅ Debugging y logging integrado

---

## 📝 Notas

- GameManager persiste entre escenas (DontDestroyOnLoad)
- Los archivos JSON son legibles y editables manualmente
- Cambiar `autoSaveInterval` en Inspector para ajustar frecuencia
- El sistema es thread-safe con JsonUtility (nativo de Unity)

---

## 🔍 Próximos Pasos (Opcionales)

- [ ] Agregar encriptación a los archivos JSON
- [ ] Crear UI de "Guardando..." visual
- [ ] Agregar backup automático
- [ ] Implementar borrado de partidas
- [ ] Agregar cloud save
- [ ] Integrar con otros sistemas (Stats, Achievements, etc)

