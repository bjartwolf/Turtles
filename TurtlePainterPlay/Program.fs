open System
open System.Windows.Forms
open System.Collections.Generic
open System.Drawing
open SharpDX.Direct2D1
open SharpDX.Direct3D10
open SharpDX.DXGI
open SharpDX.Windows
// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

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
                SharpDX.Direct3D10.FeatureLevel.Level_10_0, &device, swapChain) 

    use d2DFactory = new SharpDX.Direct2D1.Factory()
    use factory = (!swapChain).GetParent<SharpDX.DXGI.Factory>()
    factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll)
    use backBuffer = SharpDX.Direct3D10.Resource.FromSwapChain<Texture2D>((!swapChain), 0)
    use renderTrgt = new RenderTargetView(device, backBuffer)
    let surface = backBuffer.QueryInterface<Surface>()
    let d2DRenderTarget = new RenderTarget(d2DFactory, surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)))
    let yellow = SharpDX.Color.Yellow
    let yellow4 = SharpDX.Color4(float32 yellow.R/255.0f, float32 yellow.G/255.0f, float32 yellow.B/255.0f/255.0f, float32 yellow.A)
    let pink = SharpDX.Color.HotPink
    let pink4 = SharpDX.Color4(float32 pink.R/255.0f, float32 pink.G/255.0f, float32 pink.B/255.0f, float32 pink.A/255.0f)
    let solidColorBrushYellow = new SolidColorBrush(d2DRenderTarget, yellow4)
    let solidColorBrushRed = new SolidColorBrush(d2DRenderTarget, pink4)
    RenderLoop.Run(form, fun _ ->
            d2DRenderTarget.BeginDraw()
//            let rect = new SharpDX.RectangleF(float32 x*3.0f,float32 y*3.0f,float32 x*3.0f+1.0f,float32 y*3.0f+1.0f)
//            d2DRenderTarget.FillRectangle(rect, solidColorBrushRed)
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
