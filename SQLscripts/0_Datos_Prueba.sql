USE BDVentas
GO

-- *****************************************************
-- DATOS ROL
-- *****************************************************
EXEC uspInsertarRol @Descripcion = 'Administrador'
EXEC uspInsertarRol @Descripcion = 'Vendedor'
EXEC uspInsertarRol @Descripcion = 'Almacenero'

-- *****************************************************
-- DATOS PERMISO
-- *****************************************************
EXEC uspInsertarPermiso @IdRol = 'R0001', @NombreMenu = 'Todos'
EXEC uspInsertarPermiso @IdRol = 'R0002', @NombreMenu = 'Ventas'

-- *****************************************************
-- DATOS EMPLEADO
-- *****************************************************
EXEC uspInsertarEmpleado 
    @Documento = '12345678', 
    @NombreCompleto = 'Juan P�rez', 
    @Correo = 'juan@example.com', 
    @Clave = 'password123', 
    @IdRol = 'R0001', 
    @Estado = 'Activo'

EXEC uspInsertarEmpleado 
    @Documento = '87654321', 
    @NombreCompleto = 'Mar�a Garc�a', 
    @Correo = 'maria@example.com', 
    @Clave = 'password456', 
    @IdRol = 'R0002', 
    @Estado = 'Activo'

EXEC uspInsertarEmpleado 
    @Documento = '11223344', 
    @NombreCompleto = 'Carlos L�pez', 
    @Correo = 'carlos@example.com', 
    @Clave = 'password789', 
    @IdRol = 'R0002', 
    @Estado = 'Activo'

-- *****************************************************
-- DATOS CATEGORIA
-- *****************************************************
EXEC uspInsertarCategoria @Descripcion = 'Electr�nicos', @Estado = 'Activo'
EXEC uspInsertarCategoria @Descripcion = 'Muebles', @Estado = 'Activo'
EXEC uspInsertarCategoria @Descripcion = 'Ropa', @Estado = 'Activo'
EXEC uspInsertarCategoria @Descripcion = 'Alimentos', @Estado = 'Activo'

-- *****************************************************
-- DATOS PRODUCTO
-- *****************************************************
EXEC uspInsertarProducto 
    @Codigo = 'XY-1JKFDSAKJ', -- DIFERENTE AL IdProducto
    @Nombre = 'Smartphone X', 
    @Descripcion = 'Tel�fono inteligente de �ltima generaci�n', 
    @IdCategoria = 'CT0001', 
    @Stock = 50, 
    @PrecioCompra = 300.00, 
    @PrecioVenta = 450.00, 
    @Estado = 'Activo'

EXEC uspInsertarProducto 
    @Codigo = 'hjhkjhkj', 
    @Nombre = 'Silla Ergon�mica', 
    @Descripcion = 'Silla de oficina ergon�mica', 
    @IdCategoria = 'CT0002', 
    @Stock = 30, 
    @PrecioCompra = 80.00, 
    @PrecioVenta = 150.00, 
    @Estado = 'Activo'

EXEC uspInsertarProducto 
    @Codigo = 'kjjlkjkljlkj', 
    @Nombre = 'Camiseta Algod�n', 
    @Descripcion = 'Camiseta 100% algod�n', 
    @IdCategoria = 'CT0003', 
    @Stock = 100, 
    @PrecioCompra = 5.00, 
    @PrecioVenta = 15.00, 
    @Estado = 'Activo'

EXEC uspInsertarProducto 
    @Codigo = 'kjlkjkljlkj', 
    @Nombre = 'Arroz Integral', 
    @Descripcion = 'Arroz integral de 1kg', 
    @IdCategoria = 'CT0004', 
    @Stock = 200, 
    @PrecioCompra = 2.00, 
    @PrecioVenta = 4.00, 
    @Estado = 'Activo'

-- *****************************************************
-- DATOS CLIENTE
-- *****************************************************
EXEC uspInsertarCliente 
    @Documento = '11111111', 
    @NombreCompleto = 'Ana Mart�nez', 
    @Correo = 'ana@email.com', 
    @Telefono = '999888777', 
    @Estado = 'Activo'

EXEC uspInsertarCliente 
    @Documento = '22222222', 
    @NombreCompleto = 'Pedro Rodr�guez', 
    @Correo = 'pedro@email.com', 
    @Telefono = '888777666', 
    @Estado = 'Activo'

EXEC uspInsertarCliente 
    @Documento = '33333333', 
    @NombreCompleto = 'Luc�a S�nchez', 
    @Correo = 'lucia@email.com', 
    @Telefono = '777666555', 
    @Estado = 'Activo'

-- *****************************************************
-- DATOS PROVEEDOR
-- *****************************************************
EXEC uspInsertarProveedor 
    @Documento = '20123456789',
    @RazonSocial = 'Distribuidora TecnoPartes S.A.',
    @Correo = 'ventas@tecnopartes.com',
    @Telefono = '(01) 555-1234',
    @Estado = 'Activo'
-- Insertar Proveedor 2
EXEC uspInsertarProveedor 
    @Documento = '20987654321',
    @RazonSocial = 'Suministros Oficina Pro E.I.R.L.',
    @Correo = 'contacto@oficinapro.com',
    @Telefono = '(01) 555-5678',
    @Estado = 'Activo'
-- Insertar Proveedor 3
EXEC uspInsertarProveedor 
    @Documento = '20456789012',
    @RazonSocial = 'Insumos Qu�micos del Per� S.A.C.',
    @Correo = 'ventas@insumosquimicos.pe',
    @Telefono = '(01) 555-9012',
    @Estado = 'Activo'
-- Insertar Proveedor 4
EXEC uspInsertarProveedor 
    @Documento = '20345678901',
    @RazonSocial = 'Maquinarias Industriales del Sur S.R.L.',
    @Correo = 'info@maquinariassur.com',
    @Telefono = '(01) 555-3456',
    @Estado = 'Activo'

-- *****************************************************
-- DATOS COMPRA
-- *****************************************************
EXEC uspInsertarCompra 
    @IdEmpleado = 'E0002', 
    @IdProveedor = 'PRV0002', 
    @TipoDocumento = 'Factura', 
    @NumeroDocumento = 'F001-001', 
    @MontoTotal = 20000.00

EXEC uspInsertarCompra 
    @IdEmpleado = 'PRV0003', 
    @IdProveedor = 2, 
    @TipoDocumento = 'Factura', 
    @NumeroDocumento = 'F001-002', 
    @MontoTotal = 5000.00

-- *****************************************************
-- DATOS DETALLE_COMPRA
-- *****************************************************
EXEC uspInsertarDetalleCompra 
    @IdCompra = 'CM0001', 
    @IdProducto = 'PRD0001', 
    @PrecioCompra = 300.00, 
    @PrecioVenta = 450.00, 
    @Cantidad = 50, 
    @MontoTotal = 15000.00

EXEC uspInsertarDetalleCompra 
    @IdCompra = 'CM0001', 
    @IdProducto = 'PRD0002', 
    @PrecioCompra = 80.00, 
    @PrecioVenta = 150.00, 
    @Cantidad = 30, 
    @MontoTotal = 2400.00

EXEC uspInsertarDetalleCompra 
    @IdCompra = 'CM0002', 
    @IdProducto = 'PRD0003', 
    @PrecioCompra = 5.00, 
    @PrecioVenta = 15.00, 
    @Cantidad = 100, 
    @MontoTotal = 500.00

-- *****************************************************
-- DATOS VENTA
-- *****************************************************
EXEC uspInsertarVenta 
    @IdEmpleado = 'E0002', 
    @TipoDocumento = 'Boleta', 
    @NumeroDocumento = 'B001-001', 
    @DocumentoCliente = '11111111', 
    @NombreCliente = 'Ana Mart�nez', 
    @MontoPago = 500.00, 
    @MontoCambio = 50.00, 
    @MontoTotal = 450.00

EXEC uspInsertarVenta 
    @IdEmpleado = 'E0002', 
    @TipoDocumento = 'Factura', 
    @NumeroDocumento = 'F001-001', 
    @DocumentoCliente = '22222222', 
    @NombreCliente = 'Pedro Rodr�guez', 
    @MontoPago = 200.00, 
    @MontoCambio = 20.00, 
    @MontoTotal = 180.00

-- *****************************************************
-- DATOS DETALLE_VENTA
-- *****************************************************
EXEC uspInsertarDetalleVenta 
    @IdVenta = 'V0001', 
    @IdProducto = 'PRD0001', 
    @PrecioUnitario = 450.00, 
    @Cantidad = 1, 
    @SubTotal = 450.00

EXEC uspInsertarDetalleVenta 
    @IdVenta = 'V0002', 
    @IdProducto = 'PRD0003', 
    @PrecioUnitario = 15.00, 
    @Cantidad = 12, 
    @SubTotal = 180.00