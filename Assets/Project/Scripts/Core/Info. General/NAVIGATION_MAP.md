# 🗺️ MAPA DE NAVEGACIÓN - Dónde encontrar todo

## 🎯 Por Objetivo

### "Necesito empezar YA" (5 minutos)
```
📍 Assets/Project/Scripts/Core/
   └─ 00_READ_ME_FIRST.md        ← Empieza aquí
   └─ QUICKSTART.md              ← GuÍa de 5 minutos
```

### "Necesito hacer funcionar" (20 minutos)
```
📍 Assets/Project/Scripts/Core/
   └─ GAMEDATA_SETUP.md          ← Guía completa paso a paso
   └─ IMPLEMENTATION_CHECKLIST   ← Checklist de verificación
```

### "Quiero entender cómo funciona" (1 hora)
```
📍 Assets/Project/Scripts/Core/
   ├─ README.md                  ← Visión general
   ├─ ARCHITECTURE.cs            ← Diagramas internos
   └─ EXTENSION_EXAMPLES.cs      ← Cómo extender
```

### "Algo no funciona" (Debugging)
```
📍 Console (Alt + `)
   └─ GameDebugger.PrintGameState()
   └─ Ver errores y logs

📍 Editor Windows
   └─ Window > GameDebugger
   └─ Herramientas visuales
```

---

## 📂 ESTRUCTURA FÍSICA

```
Assets/Project/Scripts/

├── 🔴 CORE/ (Nuevos scripts + Documentación)
│   ├── GameData.cs                        ← Datos serializable
│   ├── GameManager.cs                     ← Singleton principal
│   ├── SaveController.cs                  ← I/O en disco
│   ├── GameRestorer.cs                    ← Restaura jugador
│   ├── GameDebugger.cs                    ← Debug tools
│   │
│   ├── 00_READ_ME_FIRST.md                ⭐ Empieza aquí
│   ├── QUICKSTART.md                      ⚡ 5 minutos
│   ├── GAMEDATA_SETUP.md                  📖 Guía completa
│   ├── IMPLEMENTATION_CHECKLIST.md        ✅ Verificación
│   ├── ARCHITECTURE.cs                    🏗️ Diagramas
│   ├── EXTENSION_EXAMPLES.cs              🔧 Extensiones
│   ├── README.md                          📚 General
│   └── DELIVERY_SUMMARY.md                📦 Resumen
│
├── 🟡 UI/ (Actualizados)
│   ├── SaveSlot.cs                        ⬆️ ACTUALIZADO
│   ├── MainMenuManager.cs                 ⬆️ ACTUALIZADO
│   └── ...
│
└── 🟢 Systems/
    └── Player/
        ├── PlayerController.cs            ⬆️ ACTUALIZADO
        └── ...
```

---

## 🔍 POR TIPO DE USUARIO

### 👨‍💼 Project Manager / Productor
```
✓ Leer: 00_READ_ME_FIRST.md (2 min)
✓ Revisar: DELIVERY_SUMMARY.md (3 min)
✓ Listo. Sistema funcionando en 5 minutos.
```

### 👨‍💻 Programador (Implementación)
```
✓ Leer: QUICKSTART.md (5 min)
✓ Seguir: GAMEDATA_SETUP.md paso a paso (20 min)
✓ Verificar: IMPLEMENTATION_CHECKLIST.md (30 min)
✓ Debugging: Usar GameDebugger cuando sea necesario
```

### 🏗️ Arquitecto de Software
```
✓ Leer: README.md (5 min)
✓ Revisar: ARCHITECTURE.cs (30 min)
✓ Estudiar: EXTENSION_EXAMPLES.cs (15 min)
✓ Evaluar: Posibilidades de extensión
```

### 🧪 QA / Tester
```
✓ Leer: IMPLEMENTATION_CHECKLIST.md (10 min)
✓ Seguir: Testing guide (15 min)
✓ Ejecutar: 7 tests de verificación
✓ Reportar: Cualquier issue encontrado
```

### 🎮 Game Designer
```
✓ Leer: QUICKSTART.md (5 min)
✓ Entender: Cómo funciona el flujo (GAMEDATA_SETUP.md)
✓ Considerar: Qué datos guardar (EXTENSION_EXAMPLES.cs)
✓ Colaborar: Con programadores para agregar datos
```

---

## 📖 DOCUMENTACIÓN POR TEMA

### "¿Cómo se guarda el juego?"
```
→ GAMEDATA_SETUP.md > Flujo de Guardado
→ ARCHITECTURE.cs > Diagrama Auto-Save
```

### "¿Cómo aparece 'Continuar'?"
```
→ QUICKSTART.md > Flujo Visual
→ GAMEDATA_SETUP.md > SaveSlots
```

### "¿Cómo se restaura el jugador?"
```
→ ARCHITECTURE.cs > Diagrama Carga
→ GAMEDATA_SETUP.md > GameRestorer
```

### "¿Cómo agrego más datos?"
```
→ EXTENSION_EXAMPLES.cs > 6 Ejemplos
→ EXTENSION_EXAMPLES.cs > Patrón General
```

### "¿Dónde se guardan los archivos?"
```
→ GAMEDATA_SETUP.md > Rutas de Guardado
→ QUICKSTART.md > Ruta de Guardos
```

### "¿Cómo debuggeo?"
```
→ GAMEDATA_SETUP.md > Debugging
→ IMPLEMENTATION_CHECKLIST.md > Debugging Avanzado
→ Abre: Window > GameDebugger
```

---

## 🚀 FLUJO POR TAREA

### Tarea: Hacer que "Continuar" aparezca

**Archivo guía:** GAMEDATA_SETUP.md > Paso 2 (SaveSlots)
```
1. Leer: "Configurar SaveSlots (UI del menú de juego)"
2. Hacer: Seguir 5 sub-pasos
3. Verificar: SaveSlots > Test 2
4. ¡Listo!
```

### Tarea: Implementar GameManager

**Archivo guía:** GAMEDATA_SETUP.md > Paso 1 (GameManager)
```
1. Leer: "Preparar la Escena MainMenu"
2. Hacer: Crear GameObject + Component
3. Verificar: Test 1
4. ¡Listo!
```

### Tarea: Verificar que todo funciona

**Archivo guía:** IMPLEMENTATION_CHECKLIST.md > Testing
```
1. Leer: "Testing - Verificar que todo funciona"
2. Ejecutar: 7 tests paso a paso
3. Documentar: Resultados
4. ¡Validado!
```

### Tarea: Agregar nuevo dato (ej: Salud)

**Archivo guía:** EXTENSION_EXAMPLES.cs > Ejemplo 3
```
1. Leer: "EJEMPLO 3: AGREGAR SALUD DEL JUGADOR"
2. Copiar: Código ejemplo
3. Adaptar: A tu caso
4. Verificar: Con GameDebugger.PrintGameState()
5. ¡Funcionando!
```

---

## 🛠️ HERRAMIENTAS DISPONIBLES

### Debug en Console
```
Command: GameDebugger.PrintGameState()
Uso: Ver estado actual de la partida

Command: GameDebugger.PrintAllSaves()
Uso: Ver todos los saves disponibles

Command: GameDebugger.ForceAutoSave()
Uso: Forzar un guardado manual

Command: GameDebugger.DeleteAllSaves()
Uso: Borrar todos los saves (¡cuidado!)

Command: GameDebugger.OpenSaveFolder()
Uso: Abrir carpeta de guardos en explorador
```

### Debug en Editor
```
Menu: Window > GameDebugger
Uso: Interfaz gráfica con todos los comandos
Permite: Ver botones, ejecutar acciones, etc.
```

### JSON Editor
```
Ubicación: %APPDATA%\..\..\LocalLow\[Empresa]\[Juego]\Saves\
Archivos: save_1.json, save_2.json, save_3.json
Editable: Sí, son JSON legibles
```

---

## 📚 CANTIDAD DE LECTURAS POR DOCUMENTO

| Documento | Páginas | Tiempo | Complejidad |
|-----------|---------|--------|-------------|
| 00_READ_ME_FIRST.md | ~2 | 5 min | Muy fácil |
| QUICKSTART.md | ~3 | 5 min | Muy fácil |
| README.md | ~4 | 10 min | Fácil |
| GAMEDATA_SETUP.md | ~8 | 20 min | Medio |
| IMPLEMENTATION_CHECKLIST.md | ~6 | 30 min | Fácil |
| ARCHITECTURE.cs | ~8 | 30 min | Difícil |
| EXTENSION_EXAMPLES.cs | ~6 | 20 min | Medio |
| DELIVERY_SUMMARY.md | ~4 | 10 min | Fácil |

---

## ✅ VERIFICACIÓN RÁPIDA

### ¿Está todo instalado?
```
Sí si vez estos archivos en Scripts/Core/:
✓ GameData.cs
✓ GameManager.cs
✓ SaveController.cs
✓ GameRestorer.cs
✓ GameDebugger.cs
```

### ¿Está configurado?
```
Ejecuta en Console: GameManager.Instance
Si ves: [Objeto GameManager]
= Está configurado correctamente
```

### ¿Funciona el auto-save?
```
Juega 70 segundos
Mira Console:
= [GameManager] AUTO-SAVE ejecutado en slot 1
```

### ¿Aparece "Continuar"?
```
Vuelve al menú
Click "Jugar"
Ve si dice: "Slot 1 - Continuar"
```

---

## 🆘 TROUBLESHOOTING RÁPIDO

### "No veo la carpeta de scripts"
```
→ Ubícate en: Assets/Project/Scripts/Core/
→ Todos los Scripts están en esa carpeta
```

### "¿Por dónde empiezo?"
```
→ Lee: 00_READ_ME_FIRST.md (está en Core/)
→ Luego: QUICKSTART.md
```

### "No entiendo cómo funciona"
```
→ Lee: README.md (visión general)
→ Luego: ARCHITECTURE.cs (diagramas)
```

### "¿Cómo agrego nuevos datos?"
```
→ Abre: EXTENSION_EXAMPLES.cs
→ Busca ejemplo similar a lo que necesitas
→ Sigue el patrón
```

### "Algo no funciona"
```
→ Abre: IMPLEMENTATION_CHECKLIST.md
→ Sección: Troubleshooting
→ O usa: GameDebugger.PrintGameState()
```

---

## 📞 REFERENCIA RÁPIDA

### Métodos Más Usados de GameManager
```csharp
GameManager.Instance.LoadGame("1");
GameManager.Instance.CreateNewGame("1");
GameManager.Instance.SaveGame();
GameManager.Instance.HasSaveFile("1");
GameManager.Instance.DebugGetGameState();
```

### Configuración Más Importante
```
GameManager Inspector:
  Auto Save Interval = 60 segundos
  Enable Auto Save = ✓

SaveSlot Inspector:
  Slot ID = 1 (2, 3 para otros slots)
  Text Label = [Tu TextMeshProUGUI]
```

### Ubicaciones Críticas
```
MainMenu Scene: GameManager aquí
GameScene: GameRestorer aquí
Saves: persistentDataPath/Saves/save_X.json
```

---

## 🎓 FLUJO RECOMENDADO DE LECTURA

```
1️⃣ 00_READ_ME_FIRST.md (5 min)
   ↓ (Entiendes qué se hizo)
2️⃣ QUICKSTART.md (5 min)
   ↓ (Sabes cómo empezar)
3️⃣ Configura en Unity (5 min)
   ↓ (Sistema funciona)
4️⃣ GAMEDATA_SETUP.md (20 min)
   ↓ (Entiendes todo en detalle)
5️⃣ IMPLEMENTATION_CHECKLIST.md (30 min)
   ↓ (Verificas todo funciona)
6️⃣ EXTENSION_EXAMPLES.cs (20 min, cuando necesites agregar datos)
   ↓ (Sabes cómo extender)
7️⃣ ARCHITECTURE.cs (30 min, si quieres Profundizar)
   ↓ (Entiendes el diseño completo)
```

---

## 🎯 METAS POR ETAPA

### Etapa 1: Setup (30 minutos)
```
✓ Leer QUICKSTART.md
✓ Crear GameManager en MainMenu
✓ Configurar 3 SaveSlots
✓ Crear GameRestorer en GameScene
✓ Play y verificar "Nueva Partida"
```

### Etapa 2: Testing (15 minutos)
```
✓ Crear una partida
✓ Esperar 70 segundos (auto-save)
✓ Volver a menú
✓ Verificar "Continuar" aparece
✓ Cargar partida guardada
```

### Etapa 3: Extensión (30 minutos)
```
✓ Leer EXTENSION_EXAMPLES.cs
✓ Agregar nuevos datos
✓ Verificar que se guardan
✓ Verificar que se cargan
```

### Etapa 4: Producción (Finalmente)
```
✓ Testing completo
✓ Consideraciones de performance
✓ Backup de saves
✓ Publicación
```

---

```
╔═════════════════════════════════════════════════════════╗
║                                                         ║
║   🗺️ MAPA COMPLETO DE NAVEGACIÓN                       ║
║                                                         ║
║   📍 Todos los archivos están en:                      ║
║   Assets/Project/Scripts/Core/                         ║
║                                                         ║
║   ⭐ Empieza por: 00_READ_ME_FIRST.md                  ║
║                                                         ║
║   ⏱️ Tiempo total de setup: 5 minutos                  ║
║   📚 Tiempo total de lectura: 2-3 horas               ║
║                                                         ║
║   ✅ LISTO PARA USAR                                   ║
║                                                         ║
╚═════════════════════════════════════════════════════════╝
```

