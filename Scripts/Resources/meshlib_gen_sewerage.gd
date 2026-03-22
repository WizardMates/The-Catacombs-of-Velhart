@tool
extends EditorScript

func _run():
	generate_mesh_library()

func generate_mesh_library():
	var mesh_library = MeshLibrary.new()
	var texture_dir = "res://Resources/Textures/Torment_Textures/True_Colour/"
	
	# Load all textures from the directory
	var textures = get_textures_from_directory(texture_dir)
	
	if textures.is_empty():
		print("No textures found in ", texture_dir)
		return
	
	var item_id = 0
	
	# Create floors (just a flat surface)
	for texture_path in textures:
		var texture = load(texture_path)
		if texture == null:
			continue
		
		# Create a plane mesh for the floor
		var plane_mesh = PlaneMesh.new()
		plane_mesh.size = Vector2(1, 1)
		
		# Create material with texture
		var material = StandardMaterial3D.new()
		material.albedo_texture = texture
		material.texture_filter = BaseMaterial3D.TEXTURE_FILTER_NEAREST
		material.cull_mode = BaseMaterial3D.CULL_DISABLED
		
		# Apply material to mesh
		var mesh_with_material = ArrayMesh.new()
		var surface = plane_mesh.get_mesh_arrays()
		mesh_with_material.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES, surface)
		mesh_with_material.surface_set_material(0, material)
		
		mesh_library.create_item(item_id)
		mesh_library.set_item_mesh(item_id, mesh_with_material)
		mesh_library.set_item_name(item_id, "Floor_" + get_texture_name(texture_path))
		
		print("Added floor: ", texture_path)
		item_id += 1
	
	# Create walls (plane rotated 90 degrees)
	for texture_path in textures:
		var texture = load(texture_path)
		if texture == null:
			continue
		
		# Create a plane mesh for the wall
		var plane_mesh = PlaneMesh.new()
		plane_mesh.size = Vector2(1, 1)
		
		# Create material with texture
		var material = StandardMaterial3D.new()
		material.albedo_texture = texture
		material.texture_filter = BaseMaterial3D.TEXTURE_FILTER_NEAREST
		material.cull_mode = BaseMaterial3D.CULL_DISABLED
		
		# Apply material to mesh
		var mesh_with_material = ArrayMesh.new()
		var surface = plane_mesh.get_mesh_arrays()
		mesh_with_material.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES, surface)
		mesh_with_material.surface_set_material(0, material)
		
		mesh_library.create_item(item_id)
		mesh_library.set_item_mesh(item_id, mesh_with_material)
		
		# Rotate the mesh itself instead
		var rotated_mesh = plane_mesh.duplicate()
		# Actually, just create the wall mesh rotated from start
		var wall_mesh = PlaneMesh.new()
		wall_mesh.size = Vector2(1, 1)
		# We'll handle rotation when placing in GridMap instead


		
		mesh_library.set_item_name(item_id, "Wall_" + get_texture_name(texture_path))
		
		print("Added wall: ", texture_path)
		item_id += 1
	
	# Save the finished library
	ResourceSaver.save(mesh_library, "res://torment_mesh_library.tres")
	print("MeshLibrary saved to res://torment_mesh_library.tres")

func get_textures_from_directory(dir_path: String) -> Array:
	var textures = []
	var dir = DirAccess.open(dir_path)
	
	if dir == null:
		print("Error: cannot open directory ", dir_path)
		return textures
	
	dir.list_dir_begin()
	var file_name = dir.get_next()
	
	# Iterate through all files in the directory
	while file_name != "":
		# Check if it's a texture file
		if file_name.ends_with(".png") or file_name.ends_with(".jpg") or \
		   file_name.ends_with(".jpeg") or file_name.ends_with(".webp"):
			textures.append(dir_path + file_name)
		file_name = dir.get_next()
	
	return textures

func get_texture_name(path: String) -> String:
	# Extract filename without extension
	var file_name = path.get_file()
	return file_name.trim_suffix("." + file_name.get_extension())
