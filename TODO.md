# TODO

## Coyote time
- Serialized `coyoteTime` (~0.1s) + private timer in `PlayerController`.
- After the ground check in `Update`: grounded → refill timer; else subtract `Time.deltaTime`.
- Jump if timer > 0, then set timer to 0.
- Gotcha: ground check still overlaps for a frame or two after takeoff and refills the timer. Use the rigidbody's vertical velocity to tell "just jumped" from "standing".
- Note: coyote jumps get no launch (`SurfaceSpeed()` is 0 when `ground` is null).
- Later: jump buffering (same timer idea, pressed just before landing).

## Instruments aim at the selected shape
- Store the selected shape from `OnSelectionChanged` in `InstrumentHolder`.
- In `LateUpdate`: `transform.InverseTransformPoint(target)` minus the instrument's offset → local direction (handles the player flip).
- `Mathf.Atan2(y, x) * Mathf.Rad2Deg` → `localRotation = Quaternion.Euler(0, 0, angle + instrument.rotation)`.
- Watch for the sprite going upside down past ±90° (`SpriteRenderer.flipY` if needed).

## Cleanup
- Confirm the EditableShape refactor compiles and boxes kept `minSize`/`maxSize`/`growDuration` in the Inspector.
- Confirm in the Vector3.Lerp docs that it clamps t (EditableShape.FixedUpdate relies on it).
- Remove the stale "force or accel to stop the stutter" comment in `PlayerController.FixedUpdate`.
- Null-check `Mouse.current` in `ScaleTool` and `SelectionManager`.
- Optional: drop Git LFS for small images (`.gitattributes` + `git add --renormalize .`).
