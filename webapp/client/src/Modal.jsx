import { useEffect, useState } from "react";

export default Modal

function Modal(props){

    const [open, setOpen] = useState(false)
    var onClose = ()=>{}

    const openButton = <button className="modalButton" onClick={()=>setOpen(true)}>{props.buttonText}</button>

    useEffect(()=>{

        onClose = () => {
            
            if(props.onClose){
                props.onClose()
            }
            setOpen(false)
        }
    })

    const dialog = 
        <dialog open id='modalDialog'>
            {props.children}
            <button onClick={()=>onClose()}>Close</button>
        </dialog>

    return(
        <>
            {openButton}
            {open? dialog:<></>}
        </>
    )

    



}