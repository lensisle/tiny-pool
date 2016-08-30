# TinyPool

Using TinyPool is very easy.

#### Initialize: 
```cs
TinyPool.Initialize();
```
#### Create a Pool: 
```cs
TinyPool.CreatePool((GameObject)Prefab, 
                    (string)PoolID, 
                    (int)InitialPoolSize, 
                    (int)maxPoolSize=50, 
                    (GameObject)objectsParent);
```
#### Spawn: 
```cs
GameObject spawned = TinyPool.Spawn((string)PoolID, 
                                    (Vector3)Position, 
                                    (Quaternion)Rotation, 
                                    (Vector3)Scale);
```
#### Destroy:
```cs
TinyPool.Destroy((string)PoolID, (GameObject)pooledObject);
```
#### Expand:
```cs
TinyPool.ExpandPool((string)PoolID, (GameObject)pooledObject);
```
### Result:

![Example](https://raw.githubusercontent.com/camiloei/TinyPool/master/image/tinypoolgif.gif)

## Installation
Just add this class to your Unity project.

## Considerations
* You have to call Initialize() just once and before any other TinyPool function.
* You can create any number of pools at the same time, just be aware of givin them a different PoolID.

### Thanks for visiting!

Feel free to use this library whatever you want whenever you want.

You can reach me at twitter @c4m170 for any questions.
