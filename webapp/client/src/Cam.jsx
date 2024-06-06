import Webcam from "react-webcam";
import {React, useState, useRef, useCallback} from 'react'

const videoConstraints = {
    width: 640,
    height: 360,
    facingMode: "user"
}


export default Cam

function Cam({frameMethod, buttonText}) {
    const webcamRef = useRef(null)
    const [showDialog, setShowDialog] = useState(false)
    const capture = useCallback(()=>{
        const img = webcamRef.current.getScreenshot()
        frameMethod(img)
    }, [webcamRef, frameMethod])
    
    return (<>
        <button onClick={()=>setShowDialog(true)}>{buttonText}</button>
        {showDialog?<dialog open>
            <Webcam
            width={640}
            height={360}
            audio={false}
            videoConstraints={videoConstraints}
            screenshotFormat="image/jpeg"
            ref={webcamRef}/><br/>
            <button onClick={capture}>Capture</button>
            <button onClick={()=>setShowDialog(false)}>Close</button>
        </dialog>:<></>}
    </>)
}
