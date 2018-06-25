# Unity_CGA

Try it out at: https://sigr3s.itch.io/a3dm-cga


## Rules

Before starting we should differenciate between the rules that are applied to the context that is aviable during all the evaluation and the rules that are applied to a certain target. For example if we apply the following rules:
      
      P(10.0,0.0,0.0)
      S(1.0,2.0,1.0)
      P(0.0,1.0,0.0)
      I("cube"){example}

First we will store the position in the context (10,0,0) then we will store the scale (1,2,1) after that we will increment the context position to (10,1,0) and finally we will instantiate a cube with the context values. Now if we apply:
  
      example=>R(0.0,45.0,40.0)

The example cube will be rotated the especified degrees but the context will remain the sema as above.

As we have seen in this examples when we specify "name=>" at the start this will be the named target to apply the instruction and when we use {destination} this will be the named tag to later on modify the instance.

### Instantiate

The instantiate rule **creates an instance** of the given object.

The string that corresponds with the instantiation is the following one:

        I("cube"){destination}
        
Note that the destination field is optional but is desirable if you want to modify it later. Also take into account that it will take the values of position, rotation and scale that are stored in the current context. One way to instantiate an object inside an already created one (getting their rotation, position and scale) is to specify that object as a target of the instantation:
        
        target=>I("cube"){destination}

The supported values inside the "" region are the ones listed in the table:

| Supported       | 
| ------------- |
| cube      | 
| cylinder      | 
| roof | 


### Position

The position rule **increments** the position in the given values.

The string that corresponds with the increment of the context position is the following one:
        
        P(1.0,2.0,0.0)

If you want to apply the position to a target (not to the context) you should specify the target:

        target=>P(1.0,2.0,0.0)
The position rule does not support a destination.

### Rotation

The rotation rule **increments** the rotation in the given values.

The string that corresponds with the increment of the context rotation is the following one:
        
        R(0.0,45.0,45.0)

If you want to apply the rotation to a target (not to the context) you should specify the target:

        target=>R(0.0,45.0,45.0)
        
The rotation rule does not support a destination.

### Scale

The position rule **sets** the scale to the given values.

The string that corresponds with the change of the scale in the context is the following one:
        
        S(1.0,2.0,1.0)

If you want to apply the scale to a target (not to the context) you should specify the target:

        target=>S(1.0,2.0,1.0)
        
The scale rule does not support a destination.

### Push/Pop

This rules are only applied to the context and are defined by the strings: 
      
      Push: [
      Pop:  ]

This operations affect the context by pushing the current context to and stack or poping a previous one.

### Subdivision

The subdivision rule **subdivides** subdivides the named object in the given parts.

The string that corresponds with the subdivision of the object from fixed values (percentages) is the following one:
        
        target=>Sub("Y",0.2,0.5,0.3){a,b,c}

The first "_" values is the axis and can be (X,Y or Z) followed by n comma separated values that are the n desired parts of the subdivision. Also it suppoorts relative values with the keyword Ir where first we have an integer number followed by an r of relative. One example will be:
        
        target=>Sub("X",2r,1r,0.3){a,b,c}

In this case the 0.7% of the remaining space will be distributed as 2*(0.7/3) for a and 1*(0.7/3) for b.
        
The subdivision rule does not support context.

## UI

![D1](https://github.com/sigr3s/Unity_CGA/blob/master/documentation/d1.PNG "")

## JSON Serializations

The rulesets can be serialized to a json format to later on reproduce them or in future version export/import them. This functionalities can be accesed from the  JSON urtilities button and then showing the current json or pasting a json and loading it.
