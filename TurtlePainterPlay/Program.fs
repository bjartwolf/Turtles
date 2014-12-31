open System
open System.Drawing
open SharpDX.Direct2D1
open SharpDX.Direct3D10
open SharpDX.DXGI
open SharpDX
open Turtles
open SharpDX.Windows

[<STAThread>]
[<EntryPoint>]
let main argv = 
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
    RenderLoop.Run(form, fun _ ->
            d2DRenderTarget.BeginDraw()
            d2DRenderTarget.Clear(new Nullable<SharpDX.Color4>(SharpDX.Color.Black.ToColor4()))
            let zeroMove = ((0.0f,0.0f),(0.0f,0.0f))
            let printSeq (seq1:seq<Line*Turtle>) = 
                let printer (lt: Line*Turtle)= 
                    let (l,t) = lt
                    let ((x1,y1),(x2,y2)) = l
                    d2DRenderTarget.DrawLine(new Vector2(x1,y1),new Vector2(x2,y2), pinkBrush,0.1f) 
                Seq.iter printer seq1; 
            for i in 3 .. 360 do
                Seq.unfold (myTurtle i) (zeroMove, (0.0, (600.0,10.0))) |> printSeq 
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
