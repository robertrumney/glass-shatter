# Glass Shatter Script

This is a Unity script that can be attached to a glass object to simulate a shattering effect when damage is applied to it. 

## Usage

1. Attach the `GlassShatter` script to a glass object in your Unity scene.
2. Set the `glass` variable to an `AudioClip` containing the sound of the glass shattering.
3. Call the `ApplyDamage(float x)` method on the object to apply damage and trigger the shattering effect.

## Details

The `GlassShatter` script defines two methods:
- `ApplyDamage(float x)`: This method is called when damage is applied to the glass object. It plays the shattering sound, breaks the glass object into triangular pieces using the `SplitMesh` coroutine, and sets a boolean flag to ensure that the shattering effect is only applied once.
- `SplitMesh(bool destroy)`: This coroutine breaks the glass object into triangular pieces. It loops through each submesh in the object's mesh and creates a new game object for each triangle in the submesh. It sets the vertices, normals, and UVs of the new game object to those of the original mesh triangle and applies an explosion force to it. Finally, it destroys the new game object after a random amount of time. After all triangles have been broken off, the renderer of the original object is disabled, and the coroutine waits for 2 seconds before destroying the original object (if the `destroy` parameter is `true`).

## License

This script is released under the MIT License. See [LICENSE](LICENSE) for details.
