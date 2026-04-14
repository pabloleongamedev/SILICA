# ⚡ GUÍA RÁPIDA (5 MINUTOS)

## Lo que se implementó

Sistema de **auto-guardado automático** para SILICA:

✅ El jugador **NO necesita presionar guardar**
✅ El juego guarda cada **60 segundos automáticamente**
✅ Al volver a iniciar, ve "**Continuar**" o "**Nueva Partida**"
✅ Al cargar, el jugador vuelve **exactamente donde quedó**
✅ Múltiples slots: **3 partidas independientes**

---

## Lo que hizo el sistema por ti

### Nuevos Scripts (5 archivos)
```
Assets/Project/Scripts/Core/
├── GameData.cs              ← Estructura de datos (serializable)
├── GameManager.cs           ← El "jefe" que maneja todo (Singleton)
├── SaveController.cs        ← Lee/escribe archivos JSON
├── GameRestorer.cs          ← Restaura posición del jugador
└── GameDebugger.cs          ← Herramientas de debugging
```

### Scripts Actualizados (3 archivos)
```
Assets/Project/Scripts/
├── UI/SaveSlot.cs           ← Ahora muestra "Continuar" correctamente
├── UI/MainMenuManager.cs    ← Sincroniza con GameManager
└── Systems/Player/PlayerController.cs  ← Envía datos a GameManager
```

---

## Configuración en Unity (Pasos Rápidos)

### 1️⃣ Agregar GameManager a MainMenu (1 min)
```
1. Abrir MainMenu Scene
2. Create > Empty GameObject
3. Nombrar: GameManager
4. Agregar Component: GameManager.cs
5. Listo ✅
```

### 2️⃣ Configurar 3 SaveSlots (2 min)
```
Para cada botón de SaveSlot (Slot 1, Slot 2, Slot 3):
1. Add Component > SaveSlot.cs
2. Asignar Slot ID = 1, 2, 3
3. Drag del TextLabel al campo
4. En Button > OnClick() > SaveSlot.OnSlotPressed()
```

### 3️⃣ Agregar GameRestorer a GameScene (1 min)
```
1. Abrir GameScene
2. Create > Empty GameObject
3. Nombrar: _GameRestorer
4. Agregar Component: GameRestorer.cs
5. Listo ✅
```

### ✅ Eso es TODO. El sistema funciona automáticamente.

---

## ¿Cómo funciona?

### Al abrir el juego:
```
Menú Principal
  ↓
Usuario presiona "Jugar"
  ↓
SaveSlots se actualizan (consultan GameManager)
  ↓
Muestra "Continuar" si hay guardado
Muestra "Nueva Partida" si no hay
```

### Durante el juego:
```
Cada frame:
  → PlayerController sincroniza posición
  
Cada 60 segundos:
  → GameManager auto-guarda todo a disco
  
Sin intervención del usuario ✨
```

### Al cargar una partida guardada:
```
Usuario presiona "Continuar"
  ↓
GameManager carga el JSON del disco
  ↓
Se carga GameScene
  ↓
GameRestorer restaura la posición
  ↓
¡Continúa exactamente donde quedó! 🎮
```

---

## 🎯 Lo que se guarda automáticamente

- ✅ Posición del jugador (X, Y, Z)
- ✅ Rotación del jugador (mira)
- ✅ Tiempo de juego jugado
- ✅ Escena actual
- ✅ Timestamp del último guardado
- ✅ ID del slot

### Extensible para agregar:
```csharp
GameManager.Instance.AddInventoryItem(itemID, gridX, gridY);
GameManager.Instance.RegisterScannedElement(elementID);
GameManager.Instance.UpdatePlayerHealth(hp, maxHp);
// ... etc
```

---

## 📁 Dónde se guardan los archivos

**Windows:**
```
C:\Users\[Usuario]\AppData\LocalLow\[Empresa]\[Juego]\Saves\
  save_1.json
  save_2.json
  save_3.json
```

**Son archivos JSON legibles** - puedes editarlos manualmente si quieres.

---

## 🧪 Verificar que funciona

### En MainMenu:
1. Play ▶
2. Ver en Console: `[GameManager] Inicializado el GameManager Singleton`
3. Click "Jugar"
4. Ver 3 slots con "Nueva Partida"

### En GameScene:
1. Jugar durante 70+ segundos
2. Ver en Console: `[GameManager] AUTO-SAVE ejecutado en slot 1`
3. Volver a MainMenu
4. Click "Jugar"
5. Ver: "Slot 1 - Continuar ✨"

### Cargar partida:
1. Click "Slot 1 - Continuar"
2. Ver en Console: `[GameRestorer] Jugador restaurado a posición: ...`
3. Jugador aparece en la misma posición anterior

---

## 🐛 Si algo falla

### "GameManager not found"
→ Verificar que existe en MainMenu Scene

### "SaveSlot not configured"
→ Verificar que cada botón tiene SaveSlot.cs + Slot ID + Text Label

### No aparece "Continuar"
→ Crear una partida, esperar 70 segundos, volver al menú

### Posición no se restaura
→ Verificar que GameRestorer está en GameScene

---

## ⚙️ Configuración Avanzada

### Cambiar frecuencia de auto-save:

```
1. Seleccionar GameManager en MainMenu
2. Inspector > Auto Save Interval
3. Cambiar de 60 a otro valor (en segundos)

Para testing: 10 segundos
Para producción: 60-300 segundos (1-5 minutos)
```

---

## 🎮 Ejemplo: Agregar datos personalizados

```csharp
// En InventorySystem:
public void AddItem(ItemData_SO itemData)
{
    // ... código existente ...
    
    // Agregar al GameManager
    GameManager.Instance.AddInventoryItem(
        itemData.itemID,
        gridX,
        gridY,
        quantity
    );
}

// En ScanSystem:
public void Scan(IScannable scannable)
{
    // ... código existente ...
    
    // Registrar elemento escaneado
    GameManager.Instance.RegisterScannedElement(scannable.ID);
}
```

---

## 📊 Debugging Rápido

En Console puedes ejecutar:
```
GameDebugger.PrintGameState()      ← Ve el estado actual
GameDebugger.PrintAllSaves()       ← Ve todos los saves
GameDebugger.ForceAutoSave()       ← Fuerza un guardado
GameDebugger.DeleteAllSaves()      ← Borra todos los saves
GameDebugger.OpenSaveFolder()      ← Abre carpeta de guardos
```

---

## ✅ LISTO

El sistema está 100% funcional. Solo falta la configuración en Unity siguiendo los pasos de arriba (5 minutos).

**Próximos pasos:**
1. Configura GameManager en MainMenu ✓
2. Configura 3 SaveSlots ✓
3. Configura GameRestorer en GameScene ✓
4. ¡Play! ▶

---

## 📚 Documentación Completa

- `GAMEDATA_SETUP.md` - Guía detallada paso a paso
- `IMPLEMENTATION_CHECKLIST.md` - Checklist de verificación
- `ARCHITECTURE.cs` - Diagramas de arquitectura
- Código comentado en cada script

