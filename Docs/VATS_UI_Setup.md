# VATS UI Button Setup Guide

This guide describes how to create the **PartButtonParent** and **PartButton** objects required by `VATSUIView` inside the Unity Editor.

## 1. Prepare the canvas hierarchy

1. Open the scene that uses `VATSUIView`.
2. Ensure there is a `Canvas` that holds the VATS UI. If the script is on a child object of the canvas (recommended), the script will fall back to its own `RectTransform` when no parent is supplied.【F:Assets/02_Scripts/_Systems/VATS/UI/VATSUIView.cs†L33-L58】
3. Under the canvas (or beneath the object that has `VATSUIView`), create an empty `RectTransform` object. Name it **PartButtonParent**. This container will receive dynamically spawned buttons and can host layout components (e.g., `VerticalLayoutGroup`, `GridLayoutGroup`) if you want an automatic arrangement.
4. Assign this transform to the **Part Button Parent** field of `VATSUIView` in the Inspector. If you omit this step, the script will use its own transform, but supplying an explicit parent makes it easier to position the buttons.

## 2. Build the part button prefab

1. In the hierarchy, create a UI `Button` under the canvas and rename it **PartButton**.
2. Add the `VATSPartButtonView` component to the button. The script expects references to `Button`, `Text`, and `Image` components so it can update the caption, respond to clicks, and show selection tinting.【F:Assets/02_Scripts/_Systems/VATS/UI/VATSPartButtonView.cs†L8-L68】
3. Inside the button:
   - Keep or replace the default `Text` child for the label. Drag this `Text` component into the **Label** field of `VATSPartButtonView`.
   - Drag the button's own `Button` component into the **Button** field (if not populated automatically).
   - Drag the background `Image` (either the same object or a dedicated child) into the **Background** field. The `VATSPartButtonView` script toggles between the normal and selected colors using this image.【F:Assets/02_Scripts/_Systems/VATS/UI/VATSPartButtonView.cs†L11-L52】
4. Configure the normal/selected colors to match your UI style. The script applies `_selectedColor` when the part is highlighted and `_normalColor` otherwise.【F:Assets/02_Scripts/_Systems/VATS/UI/VATSPartButtonView.cs†L44-L50】
5. (Optional) Customize the layout (e.g., add icons, additional text). Ensure the `VATSPartButtonView` still has valid references to the `Button`, `Text`, and `Image` components.
6. Drag the configured button into the Project window to create a prefab, then remove the temporary instance from the scene if it is only used as a prefab.

## 3. Hook up the prefab

1. Assign the new prefab to the **Part Button Prefab** field of `VATSUIView` in the Inspector.【F:Assets/02_Scripts/_Systems/VATS/UI/VATSUIView.cs†L13-L75】
2. Enter Play mode and verify that the runtime spawns buttons for every `IVATSPart` provided by the target and that clicking them invokes the callback registered through `BindParts` (the controller subscribes to this event).【F:Assets/02_Scripts/_Systems/VATS/UI/VATSUIView.cs†L61-L108】

## 4. Confirm the controller reference

`VATSController` needs a reference to an object that implements `IVATSUIView`. There are two ways to satisfy this dependency:

1. **Manual assignment** – Drag the `VATSUIView` component (or a custom script that implements `IVATSUIView`) into the **Vats UIView Behaviour** field on `VATSController`.
2. **Automatic discovery** – Leave the field empty. At runtime the controller searches the assigned **Vats UI** object and its children for a component that implements `IVATSUIView` and binds the first one it finds.【F:Assets/02_Scripts/_Controllers/VATSController.cs†L24-L108】

If neither of these approaches yields a valid implementation the controller logs an error and the UI will not update. Double-check that the VATS UI object contains a `VATSUIView` (or another class implementing the interface) and that it is active in the hierarchy.【F:Assets/02_Scripts/_Controllers/VATSController.cs†L24-L108】

With these references assigned, `VATSUIView` can create, position, and manage the buttons automatically at runtime.【F:Assets/02_Scripts/_Systems/VATS/UI/VATSUIView.cs†L59-L208】
