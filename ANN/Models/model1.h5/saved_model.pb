�P
 "serve*2.16.08�=
:
dense_4/bias:VarHandleOp*
dtype0*
shape:
O
!dense_4/bias:/Read/ReadVariableOpReadVariableOpdense_4/bias:*
dtype0
@
dense_4/kernel:VarHandleOp*
dtype0*
shape
:
S
#dense_4/kernel:/Read/ReadVariableOpReadVariableOpdense_4/kernel:*
dtype0
:
dense_3/bias:VarHandleOp*
shape:*
dtype0
O
!dense_3/bias:/Read/ReadVariableOpReadVariableOpdense_3/bias:*
dtype0
@
dense_3/kernel:VarHandleOp*
dtype0*
shape
: 
S
#dense_3/kernel:/Read/ReadVariableOpReadVariableOpdense_3/kernel:*
dtype0
:
dense_2/bias:VarHandleOp*
dtype0*
shape: 
O
!dense_2/bias:/Read/ReadVariableOpReadVariableOpdense_2/bias:*
dtype0
@
dense_2/kernel:VarHandleOp*
shape
:@ *
dtype0
S
#dense_2/kernel:/Read/ReadVariableOpReadVariableOpdense_2/kernel:*
dtype0
:
dense_1/bias:VarHandleOp*
dtype0*
shape:@
O
!dense_1/bias:/Read/ReadVariableOpReadVariableOpdense_1/bias:*
dtype0
A
dense_1/kernel:VarHandleOp*
shape:	�@*
dtype0
S
#dense_1/kernel:/Read/ReadVariableOpReadVariableOpdense_1/kernel:*
dtype0
9
dense/bias:VarHandleOp*
dtype0*
shape:�
K
dense/bias:/Read/ReadVariableOpReadVariableOpdense/bias:*
dtype0
?
dense/kernel:VarHandleOp*
dtype0*
shape:	�
O
!dense/kernel:/Read/ReadVariableOpReadVariableOpdense/kernel:*
dtype0

NoOpNoOp
�
ConstConst*
dtype0*�
value�B��"�
�
layer_with_weights-0
layer-0
layer_with_weights-1
layer-1
layer_with_weights-2
layer-2
layer_with_weights-3
layer-3
layer_with_weights-4
layer-4
non_trainable_variables

layers
	variables
	trainable_variables

	keras_api
v
non_trainable_variables

layers
	variables
trainable_variables
	keras_api

kernel
bias
v
non_trainable_variables

layers
	variables
trainable_variables
	keras_api

kernel
bias
v
non_trainable_variables

layers
	variables
trainable_variables
	keras_api

kernel
bias
v
 non_trainable_variables

!layers
"	variables
#trainable_variables
$	keras_api

%kernel
&bias
v
'non_trainable_variables

(layers
)	variables
*trainable_variables
+	keras_api

,kernel
-bias
 
 
 
 
 
 
 
 
 
 
[Y
VARIABLE_VALUEdense/kernel::06layer_with_weights-0/kernel/.ATTRIBUTES/VARIABLE_VALUE
WU
VARIABLE_VALUEdense/bias::04layer_with_weights-0/bias/.ATTRIBUTES/VARIABLE_VALUE
 
 
 
 
 
][
VARIABLE_VALUEdense_1/kernel::06layer_with_weights-1/kernel/.ATTRIBUTES/VARIABLE_VALUE
YW
VARIABLE_VALUEdense_1/bias::04layer_with_weights-1/bias/.ATTRIBUTES/VARIABLE_VALUE
 
 
 
 
 
][
VARIABLE_VALUEdense_2/kernel::06layer_with_weights-2/kernel/.ATTRIBUTES/VARIABLE_VALUE
YW
VARIABLE_VALUEdense_2/bias::04layer_with_weights-2/bias/.ATTRIBUTES/VARIABLE_VALUE
 
 
 
 
 
][
VARIABLE_VALUEdense_3/kernel::06layer_with_weights-3/kernel/.ATTRIBUTES/VARIABLE_VALUE
YW
VARIABLE_VALUEdense_3/bias::04layer_with_weights-3/bias/.ATTRIBUTES/VARIABLE_VALUE
 
 
 
 
 
][
VARIABLE_VALUEdense_4/kernel::06layer_with_weights-4/kernel/.ATTRIBUTES/VARIABLE_VALUE
YW
VARIABLE_VALUEdense_4/bias::04layer_with_weights-4/bias/.ATTRIBUTES/VARIABLE_VALUE
*
saver_filenamePlaceholder*
dtype0
8
Const_1Const*
dtype0*
valueB B^s3://.*
9
RegexFullMatchRegexFullMatchsaver_filenameConst_1
5
Const_2Const*
dtype0*
valueB B.part
:
Const_3Const*
valueB B
_temp/part*
dtype0
;
SelectSelectRegexFullMatchConst_2Const_3*
T0
9

StringJoin
StringJoinsaver_filenameSelect*
N
4

num_shardsConst*
value	B :*
dtype0
1
Const_4Const*
dtype0*
value	B : 
C
ShardedFilenameShardedFilename
StringJoinConst_4
num_shards
�
SaveV2/tensor_namesConst*
dtype0*�
value�B�B6layer_with_weights-0/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-0/bias/.ATTRIBUTES/VARIABLE_VALUEB6layer_with_weights-1/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-1/bias/.ATTRIBUTES/VARIABLE_VALUEB6layer_with_weights-2/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-2/bias/.ATTRIBUTES/VARIABLE_VALUEB6layer_with_weights-3/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-3/bias/.ATTRIBUTES/VARIABLE_VALUEB6layer_with_weights-4/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-4/bias/.ATTRIBUTES/VARIABLE_VALUEB_CHECKPOINTABLE_OBJECT_GRAPH
X
SaveV2/shape_and_slicesConst*)
value BB B B B B B B B B B B *
dtype0
�
SaveV2SaveV2ShardedFilenameSaveV2/tensor_namesSaveV2/shape_and_slices!dense/kernel:/Read/ReadVariableOpdense/bias:/Read/ReadVariableOp#dense_1/kernel:/Read/ReadVariableOp!dense_1/bias:/Read/ReadVariableOp#dense_2/kernel:/Read/ReadVariableOp!dense_2/bias:/Read/ReadVariableOp#dense_3/kernel:/Read/ReadVariableOp!dense_3/bias:/Read/ReadVariableOp#dense_4/kernel:/Read/ReadVariableOp!dense_4/bias:/Read/ReadVariableOpConst*
dtypes
2
.
Const_5Const*
valueB *
dtype0
Q
&MergeV2Checkpoints/checkpoint_prefixesPackShardedFilename*
N*
T0
Y
MergeV2CheckpointsMergeV2Checkpoints&MergeV2Checkpoints/checkpoint_prefixesConst_5
B
IdentityIdentitysaver_filename^MergeV2Checkpoints*
T0
�
RestoreV2/tensor_namesConst*
dtype0*�
value�B�B6layer_with_weights-0/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-0/bias/.ATTRIBUTES/VARIABLE_VALUEB6layer_with_weights-1/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-1/bias/.ATTRIBUTES/VARIABLE_VALUEB6layer_with_weights-2/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-2/bias/.ATTRIBUTES/VARIABLE_VALUEB6layer_with_weights-3/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-3/bias/.ATTRIBUTES/VARIABLE_VALUEB6layer_with_weights-4/kernel/.ATTRIBUTES/VARIABLE_VALUEB4layer_with_weights-4/bias/.ATTRIBUTES/VARIABLE_VALUEB_CHECKPOINTABLE_OBJECT_GRAPH
[
RestoreV2/shape_and_slicesConst*)
value BB B B B B B B B B B B *
dtype0
u
	RestoreV2	RestoreV2saver_filenameRestoreV2/tensor_namesRestoreV2/shape_and_slices*
dtypes
2
*

Identity_1Identity	RestoreV2*
T0
L
AssignVariableOpAssignVariableOpdense/kernel:
Identity_1*
dtype0
,

Identity_2IdentityRestoreV2:1*
T0
L
AssignVariableOp_1AssignVariableOpdense/bias:
Identity_2*
dtype0
,

Identity_3IdentityRestoreV2:2*
T0
P
AssignVariableOp_2AssignVariableOpdense_1/kernel:
Identity_3*
dtype0
,

Identity_4IdentityRestoreV2:3*
T0
N
AssignVariableOp_3AssignVariableOpdense_1/bias:
Identity_4*
dtype0
,

Identity_5IdentityRestoreV2:4*
T0
P
AssignVariableOp_4AssignVariableOpdense_2/kernel:
Identity_5*
dtype0
,

Identity_6IdentityRestoreV2:5*
T0
N
AssignVariableOp_5AssignVariableOpdense_2/bias:
Identity_6*
dtype0
,

Identity_7IdentityRestoreV2:6*
T0
P
AssignVariableOp_6AssignVariableOpdense_3/kernel:
Identity_7*
dtype0
,

Identity_8IdentityRestoreV2:7*
T0
N
AssignVariableOp_7AssignVariableOpdense_3/bias:
Identity_8*
dtype0
,

Identity_9IdentityRestoreV2:8*
T0
P
AssignVariableOp_8AssignVariableOpdense_4/kernel:
Identity_9*
dtype0
-
Identity_10IdentityRestoreV2:9*
T0
O
AssignVariableOp_9AssignVariableOpdense_4/bias:Identity_10*
dtype0

NoOp_1NoOp
�
Identity_11Identitysaver_filename^AssignVariableOp^AssignVariableOp_1^AssignVariableOp_2^AssignVariableOp_3^AssignVariableOp_4^AssignVariableOp_5^AssignVariableOp_6^AssignVariableOp_7^AssignVariableOp_8^AssignVariableOp_9^NoOp_1*
T0 "�-
saver_filename:0
Identity:0Identity_118"
saved_model_main_op :�
�
layer_with_weights-0
layer-0
layer_with_weights-1
layer-1
layer_with_weights-2
layer-2
layer_with_weights-3
layer-3
layer_with_weights-4
layer-4
non_trainable_variables

layers
	variables
	trainable_variables

	keras_api"
_tf_keras_layer
�
non_trainable_variables

layers
	variables
trainable_variables
	keras_api

kernel
bias"
_tf_keras_layer
�
non_trainable_variables

layers
	variables
trainable_variables
	keras_api

kernel
bias"
_tf_keras_layer
�
non_trainable_variables

layers
	variables
trainable_variables
	keras_api

kernel
bias"
_tf_keras_layer
�
 non_trainable_variables

!layers
"	variables
#trainable_variables
$	keras_api

%kernel
&bias"
_tf_keras_layer
�
'non_trainable_variables

(layers
)	variables
*trainable_variables
+	keras_api

,kernel
-bias"
_tf_keras_layer
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
:	�2dense/kernel
:�2
dense/bias
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
!:	�@2dense_1/kernel
:@2dense_1/bias
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
 :@ 2dense_2/kernel
: 2dense_2/bias
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
 : 2dense_3/kernel
:2dense_3/bias
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
"
_generic_user_object
 :2dense_4/kernel
:2dense_4/bias