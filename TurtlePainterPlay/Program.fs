module myStuff

open System
open System.Windows.Forms
open System.Collections.Generic
open System.Drawing
open SharpDX.Direct2D1
open SharpDX
open SharpDX.Direct3D10
open SharpDX.DXGI
open SharpDX.Windows
// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

type Point = {x: single; y: single}
type Line = {p1: Point; p2: Point}

let drawPinkLine(trgt: RenderTarget) (line: Line): unit =
    let {p1=p1; p2= p2} = line
    let {x=x1; y=y1} = p1 
    let {x=x2; y=y2} = p2 
    let pink = SharpDX.Color.HotPink
    let pink4 = SharpDX.Color4(float32 pink.R/255.0f, float32 pink.G/255.0f, float32 pink.B/255.0f, float32 pink.A/255.0f)
    let pinkBrush = new SolidColorBrush(trgt, pink4)
    trgt.DrawLine(new Vector2(x1,y1), new Vector2(x2,y2), pinkBrush)
    

[<STAThread>]
[<EntryPoint>]
let main argv = 
    let form = new RenderForm("Turtles!", Size = new Size(800, 800))
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
    
    use loop = new RenderLoop(form)
    form.Show()
    let stopWatch = new System.Diagnostics.Stopwatch()
    stopWatch.Start()
    let painter = MailboxProcessor<Line>.Start(fun inbox -> 
                    let rec loop n =
                        async { let! msg = inbox.Receive(10)
                                d2DRenderTarget.BeginDraw()
                                drawPinkLine d2DRenderTarget msg
                                d2DRenderTarget.EndDraw()
                                (!swapChain).Present(0, PresentFlags.None)
                                return! loop 0 }
                    loop 0)
    for i in 1 .. 1000000 do
        let myline2 : Line= {p1 = {x = (float32 i)/5.0f;y = 10.0f}; p2 = {x = 200.0f;y = 300.0f} }
        painter.Post(myline2)
    Console.WriteLine("DoNe")
    Console.ReadLine() |> ignore
    backBuffer.Dispose()
    device.ClearState()
    device.Flush()
    device.Dispose()
    device.Dispose()
    (!swapChain).Dispose()
    0 // return an integer exit code
