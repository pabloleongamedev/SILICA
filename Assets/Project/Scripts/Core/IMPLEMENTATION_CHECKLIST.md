# ✅ CHECKLIST DE IMPLEMENTACIÓN - Sistema de Guardado Automático

## 📂 Archivos Creados Automáticamente
- ✅ `GameData.cs` - Estructura de datos
- ✅ `GameManager.cs` - Gestor principal (Singleton)
- ✅ `SaveController.cs` - I/O en disco
- ✅ `GameRestorer.cs` - Restaura estado del jugador
- ✅ `GameDebugger.cs` - Herramientas de debugging
- ✅ `SaveSlot.cs` (ACTUALIZADO)
- ✅ `MainMenuManager.cs` (ACTUALIZADO)
- ✅ `PlayerController.cs` (ACTUALIZADO)

---

## 🔧 CONFIGURACIÓN EN UNITY - PASO A PASO

### PASO 1: Preparar la Escena MainMenu
- [ ] Abrir escena: `Assets/Project/Scenes/MainMenu`
- [ ] Crear nuevo GameObject vacío
  - [ ] Nombre: `GameManager`
  - [ ] Position: 0, 0, 0
  - [ ] No necesita transform especial
- [ ] Agregar Component: `GameManager.cs`
- [ ] En el Inspector de GameManager:
  - [ ] Auto Save Interval: `60` (segundos)
  - [ ] Enable Auto Save: `✓` (marcado)
- [ ] Arrastrar GameManager al campo DontDestroyOnLoad (debería hacerlo automáticamente)

### PASO 2: Configurar SaveSlots (UI del menú de juego)
- [ ] Ir a Canvas > Panels > PlayPanel (o donde esté el panel de juego)
- [ ] Encontrar los 3 botones de SaveSlot (Slot 1, Slot 2, Slot 3)

⚠️ **Para cada SaveSlot:**
1. [ ] Seleccionar el Button (Slot 1)
2. [ ] Agregar Component: `SaveSlot.cs`
3. [ ] En el Inspector:
   - [ ] Slot ID: `1` (diferente para cada slot)
   - [ ] Text Label: Arrastrar el TextMeshProUGUI del botón
   - [ ] Info Label: (Opcional) Arrastrar texto adicional si existe
4. [ ] En Button > On Click ():
   - [ ] Click el "+" para agregar evento
   - [ ] Drag el GameObject con SaveSlot.cs al campo
   - [ ] Dropdown > SaveSlot > OnSlotPressed()
5. [ ] Repetir pasos 1-4 para Slot 2 (Slot ID: 2) y Slot 3 (Slot ID: 3)

**Verificar:**
- [ ] Cada botón tiene SaveSlot.cs
- [ ] Cada SaveSlot tiene un Slot ID único (1, 2, 3)
- [ ] Cada SaveSlot tiene Text Label asignado
- [ ] Cada botón tiene On Click() → SaveSlot.OnSlotPressed()

### PASO 3: Crear GameRestorer en la Escena de Juego
- [ ] Abrir escena: `Assets/Project/Scenes/GameScene` (o la escena principal del juego)
- [ ] Crear GameObject vacío
  - [ ] Nombre: `_GameRestorer`
  - [ ] Position: 0, 0, 0
- [ ] Agregar Component: `GameRestorer.cs`
- [ ] NO configurar nada más, se ejecuta automáticamente

### PASO 4: Verificar Configuración de MainMenuManager
- [ ] Seleccionar Canvas en MainMenu
- [ ] Buscar script: `MainMenuManager`
- [ ] En el Inspector verificar:
  - [ ] Main Panel: Asignado
  - [ ] Play Panel: Asignado
  - [ ] Options Panel: Asignado
  - [ ] Credits Panel: Asignado
  - [ ] Main First Button: Asignado
  - [ ] Play First Button: Asignado

### PASO 5: Conectar Botones del Menú
- [ ] Botón "Jugar":
  - [ ] On Click() > Drag MainMenuManager
  - [ ] Dropdown > MainMenuManager > ShowPlayMenu()
- [ ] Botón "Opciones":
  - [ ] On Click() > Drag MainMenuManager
  - [ ] Dropdown > MainMenuManager > ShowOptions()
- [ ] Botón "Créditos":
  - [ ] On Click() > Drag MainMenuManager
  - [ ] Dropdown > MainMenuManager > ShowCredits()
- [ ] Botones "Atrás" (en cada panel):
  - [ ] On Click() > Drag MainMenuManager
  - [ ] Dropdown > MainMenuManager > ShowMainMenu()

### PASO 6: Configurar PlayerController
- [ ] Encontrar PlayerController en GameScene
  - [ ] Seleccionar GameObject con tag "Player"
  - [ ] Verificar que tiene el script `PlayerController`
- [ ] Verificar que tiene `MovementController` (automático)
- [ ] Verificar que tiene `MouseLook` asignado (automático)
- [ ] ✅ PlayerController ya está actualizado para sincronizar con GameManager

---

## 🎮 TESTING - VERIFICAR QUE TODO FUNCIONA

### Test 1: Verificar GameManager se crea
- [ ] Play en MainMenu
- [ ] Consola debe mostrar: `[GameManager] Inicializado el GameManager Singleton`
- [ ] No hay errores NullReference

### Test 2: Ver "Nueva Partida" en todos los slots
- [ ] Play en MainMenu
- [ ] Click "Jugar"
- [ ] Verificar que aparecen 3 botones:
  - [ ] Slot 1 - Nueva Partida
  - [ ] Slot 2 - Nueva Partida
  - [ ] Slot 3 - Nueva Partida
- [ ] Info Label muestra "Vacío" (si está configurado)

### Test 3: Crear una partida
- [ ] Click "Slot 1 - Nueva Partida"
- [ ] Consola debe mostrar: `[GameManager] Nueva partida creada en slot 1`
- [ ] GameScene carga automáticamente
- [ ] No hay errores

### Test 4: Auto-save funciona
- [ ] Jugar durante 70+ segundos
- [ ] Consola debe mostrar: `[GameManager] AUTO-SAVE ejecutado en slot 1`
- [ ] Verificar que en archivo existe: `%LocalAppData%\..\..\LocalLow\[Empresa]\[Juego]\Saves\save_1.json`

### Test 5: Ver "Continuar" en el menú
- [ ] Mientras está en juego, ir a MainMenu (o crear escena de pausa)
- [ ] Click "Jugar"
- [ ] Verificar:
  - [ ] Slot 1 - Continuar (con tiempo de juego y fecha)
  - [ ] Slot 2 - Nueva Partida
  - [ ] Slot 3 - Nueva Partida

### Test 6: Cargar partida guardada
- [ ] Click "Slot 1 - Continuar"
- [ ] Consola debe mostrar: `[GameManager] Partida cargada del slot 1`
- [ ] GameScene carga
- [ ] GameRestorer restaura la posición del jugador
- [ ] Consola debe mostrar: `[GameRestorer] Jugador restaurado a posición: ...`

### Test 7: Crear otra partida en un slot diferente
- [ ] Volver al menú
- [ ] Click "Jugar" > "Slot 2 - Nueva Partida"
- [ ] Esperar 70+ segundos (auto-save en slot 2)
- [ ] Volver al menú
- [ ] Click "Jugar"
- [ ] Verificar:
  - [ ] Slot 1 - Continuar (partida anterior)
  - [ ] Slot 2 - Continuar (partida nueva)
  - [ ] Slot 3 - Nueva Partida

---

## 🐛 DEBUGGING - Si Algo NO Funciona

### Error: "GameManager not found"
- [ ] Verificar que existe GameObject "GameManager" en MainMenu
- [ ] Verificar que tiene script `GameManager.cs`
- [ ] Verificar que está habilitado (checkbox en Inspector)

### Error: "SaveSlot not found"
- [ ] Verificar que cada botón tiene `SaveSlot.cs`
- [ ] Verificar que SaveSlot.cs está en la carpeta correcta
- [ ] Revisar Console para errors de compilación

### No aparece "Continuar" en los slots
- [ ] Ejecutar: `GameDebugger.PrintAllSaves()` en Console
- [ ] Si dice "vacío", es correcto (primera vez)
- [ ] Crear una partida y esperar 70 segundos
- [ ] Volver al menú y verificar de nuevo

### Auto-save NO funciona
- [ ] Verificar que `Enable Auto Save = true` en GameManager
- [ ] Verificar que `Auto Save Interval = 60` (o menos para testing)
- [ ] Ver Console: debe haber `[GameManager] AUTO-SAVE ejecutado` cada 60s
- [ ] Si no hay, revisar que `isInGame` es true

### Archivo JSON no se crea
- [ ] Abrir: `GameDebugger.PrintSavePath()` en Console
- [ ] Copiar la ruta que muestra
- [ ] Abrir en File Explorer
- [ ] Verificar que existe carpeta "Saves"
- [ ] Si no existe, crear manualmente (debe crearse sola)

### Posición del jugador no se restaura
- [ ] Verificar que `GameRestorer` existe en GameScene
- [ ] Verificar que se ejecuta en Awake (Console muestra log)
- [ ] Verificar que PlayerController se encuentra con `FindObjectOfType`
- [ ] Si no se encuentra, revisar que tiene el tag "Player" o está habilitado

---

## 📊 DEBUGGING AVANZADO

### Ver estado completo del juego
```
Console > GameDebugger.PrintGameState()
```
Muestra:
- Slot ID
- Escena
- Tiempo jugado
- Items en inventario
- Elementos escaneados
- Posición del jugador

### Ver información de todos los saves
```
Console > GameDebugger.PrintAllSaves()
```

### Forzar auto-save manual
```
Console > GameDebugger.ForceAutoSave()
```

### Abrir carpeta de guardos
```
Console > GameDebugger.OpenSaveFolder()
```

### Eliminar todos los saves
```
Console > GameDebugger.DeleteAllSaves()
```

---

## 🎯 PUNTOS CLAVE A RECORDAR

1. **GameManager** debe estar en MainMenu, NO en GameScene
2. **GameRestorer** debe estar en GameScene, NO en MainMenu
3. Cada **SaveSlot** debe tener un **Slot ID único** (1, 2, 3)
4. Los archivos JSON se crean a `Application.persistentDataPath/Saves/`
5. Auto-save es **cada 60 segundos** configurables
6. El sistema es **automático**, no requiere botón "Guardar"
7. Al volver al menú, los slots muestran "Continuar" si hay guardado

---

## ✅ CONFIRMACIÓN FINAL

- [ ] Todos los archivos .cs están creados
- [ ] GameManager está en MainMenu
- [ ] GameRestorer está en GameScene
- [ ] Todos los SaveSlots están configurados (3 slots)
- [ ] MainMenuManager está configurado
- [ ] PlayerController está actualizado
- [ ] No hay errores de compilación
- [ ] Test 1-7 pasan correctamente
- [ ] ¡Sistema listo para usar! 🎉

---

## 📞 SOPORTE

Si encuentras problemas:
1. Abre Console (Ctrl + `)
2. Ejecuta: `GameDebugger.PrintGameState()`
3. Verifica los logs para mensajes de error
4. Revisa que todos los GameObjects y Scripts estén asignados

