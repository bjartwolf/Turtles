open System
open System.Drawing
open SharpDX.Direct2D1
open SharpDX.Direct3D10
open SharpDX.DXGI
open SharpDX
open Turtles
open SharpDX.Windows
open SharpDX.Direct2D1.Effects

[<STAThread>]
[<EntryPoint>]
let main argv = 
    // 24 x 15
    let form = new RenderForm("SharpDX - MiniTri Direct2D - Direct3D 10 Sample", Size = new Size(1500, 800))
    let dxtrue = new SharpDX.Bool(true)
    let desc = new SwapChainDescription (
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = dxtrue,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput)
    let mutable device: SharpDX.Direct3D10.Device1 = null
    let swapChain = ref null 
    SharpDX.Direct3D10.Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc,
                SharpDX.Direct3D10.FeatureLevel.Level_10_1, &device, swapChain) 

    use d2DFactory = new SharpDX.Direct2D1.Factory()
    use factory = (!swapChain).GetParent<SharpDX.DXGI.Factory>()
    factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll)
    use backBuffer = SharpDX.Direct3D10.Resource.FromSwapChain<Texture2D>((!swapChain), 0)
    use renderTrgt = new RenderTargetView(device, backBuffer)
    let surface = backBuffer.QueryInterface<Surface>()
    let d2DRenderTarget = new RenderTarget(d2DFactory, surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)))
    let yellow = SharpDX.Color.Yellow
    
    let pinkBrush = new SolidColorBrush(d2DRenderTarget, SharpDX.Color.HotPink.ToColor4())
    let j = ref 0.0

    RenderLoop.Run(form, fun _ ->
            let rot = !j
            j := rot + 0.001
            d2DRenderTarget.BeginDraw()
            let myAngle : float32 = float32 rot 
            let center = new Vector2(float32 500.0,float32 300.0)
            let scale = new Vector2(float32 (-rot+1.0),float32 (-rot+1.0))
            d2DRenderTarget.Transform <- Matrix3x2.Scaling(scale)*Matrix3x2.Translation(-center)*Matrix3x2.Rotation(myAngle)*Matrix3x2.Translation(center)
            let printLines (lines:seq<Line option>) = 
                let printLine (l: Line) = 
                    let ((x1,y1),(x2,y2)) = l 
//                    d2DRenderTarget.DrawLine(new Vector2(x1,y1),new Vector2(x2,y2), pinkBrush,0.3f) 
                    d2DRenderTarget.DrawLine(new Vector2(x1,y1),new Vector2(x2,y2), pinkBrush,0.2f) 
                for line in lines do
                    match line with 
                        | None -> ()
                        | Some(l) -> printLine l 
            d2DRenderTarget.Clear(new Nullable<SharpDX.Color4>(SharpDX.Color.Black.ToColor4()))
//            largeSimpleTurtle 271 (0.0, (float 500.0,300.0)) |> printLines
            for x in 1 ..1.. 24 do // 24*15 = 360
                for y in 1 ..2.. 15 do
                      simpleTurtle (x*15+y) (0.0, (float (x+1) * 50.0,float (y-1) *50.0)) |> printLines
//                      turtlePoly (x*15+y) (0.0, (float (x+1) * 50.0,float (y-1) *50.0)) |> printLines
            d2DRenderTarget.EndDraw()
            (!swapChain).Present(0, PresentFlags.None)
        )
    printfn "%A" argv
    backBuffer.Dispose()
    device.ClearState()
    device.Flush()
    device.Dispose()
    device.Dispose()
    (!swapChain).Dispose()
    0 // return an integer exit code
