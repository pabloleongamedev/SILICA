# 🎮 Sistema de Guardado Automático - SILICA

## 📌 ¿Qué es esto?

Sistema completo de **auto-guardado automático** para el juego SILICA:

- ✅ **Sin botón de guardar** - El juego guarda automáticamente
- ✅ **Cada 60 segundos** - Periódicamente durante el juego
- ✅ **Botón "Continuar"** - Aparece cuando hay partida guardada
- ✅ **Restauración automática** - El jugador vuelve donde quedó
- ✅ **3 slots independientes** - Múltiples partidas guardadas
- ✅ **Completamente escalable** - Fácil de agregar nuevos datos

---

## 📂 Archivos del Sistema

### 🔧 Core Scripts (Motores del Sistema)

| Archivo | Propósito | Ubicación |
|---------|-----------|-----------|
| **GameData.cs** | Estructura serializable de todos los datos | `Scripts/Core/` |
| **GameManager.cs** | Singleton que orquesta auto-save (60s) | `Scripts/Core/` |
| **SaveController.cs** | Maneja I/O (lectura/escritura JSON) | `Scripts/Core/` |
| **GameRestorer.cs** | Restaura jugador al cargar partida | `Scripts/Core/` |
| **GameDebugger.cs** | Herramientas de debugging (Editor Window) | `Scripts/Core/` |

### 🎮 Scripts Actualizados (Integración)

| Archivo | Cambios |
|---------|---------|
| **SaveSlot.cs** | Ahora usa GameManager para detectar guardos ✨ |
| **MainMenuManager.cs** | Sincroniza estados de guardos |
| **PlayerController.cs** | Envía posición/rotación a GameManager |

### 📖 Documentación

| Archivo | Contenido | Para quién |
|---------|----------|-----------|
| **QUICKSTART.md** | Explicación de 5 minutos ⚡ | Todos |
| **GAMEDATA_SETUP.md** | Guía completa paso a paso | Implementadores |
| **IMPLEMENTATION_CHECKLIST.md** | Checklist de verificación | QA/Testing |
| **ARCHITECTURE.cs** | Diagramas y flujos completos | Arquitectos/Dev |
| **ARCHITECTURE_DIAGRAMS.md** | Diagramas ASCII explicados | Diseñadores |

---

## 🚀 Inicio Rápido (5 Minutos)

### Paso 1: GameManager en MainMenu (1 min)
```
1. MainMenu Scene
2. Create > Empty GameObject
3. Nombre: "GameManager"
4. Add Component: GameManager.cs
✓ Listo
```

### Paso 2: SaveSlots Configurados (2 min)
```
Para cada botón de Slot (1, 2, 3):
1. Add Component: SaveSlot.cs
2. Slot ID = 1 (o 2, 3)
3. Drag Text Label al campo
4. Button > OnClick() > SaveSlot.OnSlotPressed()
✓ Listo
```

### Paso 3: GameRestorer en GameScene (1 min)
```
1. GameScene
2. Create > Empty GameObject
3. Nombre: "_GameRestorer"
4. Add Component: GameRestorer.cs
✓ Listo
```

### ✨ El sistema funciona automáticamente

---

## 🎯 Cómo Funciona

### Durante la Sesión

```
USUARIO ABRE JUEGO
    ↓
GameManager se crea (Singleton)
    ↓
Usuario presiona "JUGAR"
    ↓
SaveSlots consultan GameManager
    ↓
Muestran "CONTINUAR" (si hay guardado)
      o "NUEVA PARTIDA" (si no hay)
    ↓
Usuario selecciona un slot
    ↓
GameManager carga JSON desde disco
    ↓
Se restaura posición/rotación del jugador
    ↓
¡JUEGO CONTINÚA DONDE QUEDÓ!
```

### Auto-Save en Background

```
JUEGO EN EJECUCIÓN
    ↓
PlayerController sincroniza posición cada 0.5s
    ↓
GameManager cuenta tiempo...
    ↓
CADA 60 SEGUNDOS:
  • Actualiza datos del jugador
  • Escribe JSON a disco
  • Log: "AUTO-SAVE ejecutado"
    ↓
REPITE AUTOMÁTICAMENTE
```

---

## 📊 Datos que se Guardan

### Automáticamente ✅

```csharp
GameData {
  // Partida
  slotID: "1"
  lastSaveTime: "2026-04-14 15:30:45"
  playTimeSeconds: 3600
  
  // Jugador
  playerData: {
    position: (10.5, 2.0, -5.3)
    rotation: (0, 0.707, 0, 0.707)
    health: 100
    jetpackFuel: 75
  }
  
  // Mundo
  currentScene: "GameScene"
  scannedElements: ["element_1", "element_2", ...]
  inventoryItems: [...]
}
```

### Extensible 📝

```csharp
// Desde cualquier script:
GameManager.Instance.UpdatePlayerHealth(hp, maxHp);
GameManager.Instance.AddInventoryItem(itemID, gridX, gridY);
GameManager.Instance.RegisterScannedElement(elementID);

// Se incluye automáticamente en próximo auto-save
```

---

## 🧪 Verificar que Funciona

### Test 1: Ver "Nueva Partida" (Primera vez)
1. Play en MainMenu ▶
2. Click "Jugar"
3. Ver 3 slots con "Nueva Partida"

### Test 2: Ver auto-save
1. Click "Nueva Partida"
2. Esperar 70 segundos
3. Console debe mostrar: `AUTO-SAVE ejecutado`

### Test 3: Ver "Continuar"
1. Volver a menú
2. Click "Jugar"
3. Ver "Slot 1 - Continuar ✅"

### Test 4: Cargar partida
1. Click "Continuar"
2. Jugador debe estar en misma posición

---

## 🐛 Debugging

### Ver estado actual
```
Console: GameDebugger.PrintGameState()
```

Output:
```
=== GAME STATE ===
Slot: 1
Escena: GameScene
Tiempo Jugado: 1h 23m
Items: 5
Posición: (10.5, 2.0, -5.3)
==================
```

### Ver todos los saves
```
Console: GameDebugger.PrintAllSaves()
```

### Forzar guardado
```
Console: GameDebugger.ForceAutoSave()
```

### Vista en Editor
```
Window > GameDebugger
(Interfaz gráfica para todos los comandos)
```

---

## 📁 Ruta de Guardos

Los archivos JSON se guardan en:

**Windows:**
```
%APPDATA%\..\..\LocalLow\[Empresa]\[Juego]\Saves\
  save_1.json
  save_2.json
  save_3.json
```

**macOS/Linux:**
```
~/.config/unity3d/[Empresa]/[Juego]/Saves/
```

**En código:**
```csharp
string path = Path.Combine(
  Application.persistentDataPath, 
  "Saves"
);
```

---

## ⚙️ Configuración

### Cambiar Frecuencia de Auto-Save

```
1. Seleccionar GameManager
2. Inspector > Auto Save Interval
3. Cambiar valor (en segundos)

Ejemplos:
• 10 = cada 10 segundos (para testing)
• 60 = cada 1 minuto (recomendado)
• 300 = cada 5 minutos (menos I/O)
```

### Desactivar Auto-Save

```
1. Seleccionar GameManager
2. Inspector > Enable Auto Save
3. Desmarcar (no recomendado)
```

---

## 🔗 Integración con Otros Sistemas

### InventorySystem

```csharp
public void AddItem(ItemData_SO itemData)
{
    grid.TryAddItem(instance);
    
    // Agregar a GameManager
    GameManager.Instance.AddInventoryItem(
        itemData.itemID,
        gridX, gridY,
        quantity
    );
}
```

### ScanSystem

```csharp
public void Scan(IScannable scannable)
{
    scannedElements.Add(scannable);
    
    // Registrar en GameManager
    GameManager.Instance.RegisterScannedElement(
        scannable.elementID
    );
}
```

### PlayerHealth

```csharp
public void TakeDamage(int damage)
{
    health -= damage;
    
    // Actualizar en GameManager
    GameManager.Instance.UpdatePlayerHealth(
        health, maxHealth
    );
}
```

---

## 📚 Documentación Detallada

### Para entender rápido:
→ Lee **QUICKSTART.md** (5 min)

### Para implementar:
→ Lee **GAMEDATA_SETUP.md** (paso a paso)

### Para verificar:
→ Usa **IMPLEMENTATION_CHECKLIST.md** (verificación)

### Para arquitectos:
→ Revisa **ARCHITECTURE.cs** (diagramas)

---

## ✨ Características

- ✅ Auto-save transparente (sin botón)
- ✅ Múltiples slots (1, 2, 3)
- ✅ "Continuar" aparece junto a "Nueva Partida"
- ✅ Restauración automática de posición
- ✅ Extensible para nuevos datos
- ✅ JSON legible (puedes editarlo)
- ✅ Debugging en Editor
- ✅ Sin dependencias externas (JsonUtility nativo)
- ✅ Multiplataforma (Windows, Mac, Linux, Mobile)

---

## 🎓 Estructura Técnica

### Patrón: Singleton + Serializable

```
┌─────────────────────┐
│  GAMEMANAGER (S)    │  ← Único en toda la app
├─────────────────────┤
│ + currentGameData   │
│ + saveController    │
│ + autoSaveInterval  │
├─────────────────────┤
│ + LoadGame()        │
│ + SaveGame()        │
│ + UpdatePlayer*()   │  ← Métodos para agregar data
└─────────────────────┘
        ↓
    ┌───────────────┐
    │  GAMEDATA     │  ← JSON serializable
    │  (S)          │
    └───────────────┘
```

---

## 🚨 Requisitos Previos

- ✅ Unity 2021+
- ✅ Script ExecutionOrder configurado (si es necesario)
- ✅ Tags/Layers si se necesita buscar objetos
- ✅ Paths de SceneManager correctos

---

## 🎯 Próximas Mejoras (Opcionales)

- [ ] Encriptación de archivos JSON
- [ ] Compresión de archivos
- [ ] Cloud save (PlayFab, Firebase)
- [ ] Backup automático
- [ ] Versioning de guardos
- [ ] Borrado de partidas (UI)
- [ ] Estadísticas (achievements, etc)

---

## ✅ Estado del Proyecto

- ✅ Análisis completado
- ✅ Arquitectura diseñada
- ✅ Código implementado (8 scripts)
- ✅ Documentación completa
- ✅ Herramientas de debugging
- ⏳ Configuración en Unity (5 minutos)
- ⏳ Testing (15 minutos)

---

## 🎮 ¡Listo para Usar!

Sigue la guía de 5 minutos arriba y el sistema estará funcionando.

**Preguntas?** Consulta la documentación en los archivos `.md` en `Scripts/Core/`

