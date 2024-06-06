import { useEffect, useState } from "react";
import Modal from './Modal'
import HOST from './index'
import Cam from './Cam'

export default Add

function Add(){
    const [name, setName] = useState(undefined)
    const [upc, setUPC] = useState(undefined)
    const [num, setNum] = useState(undefined)

    const onSubmit = async (e) =>{
        await fetch(`${HOST}/add`, {
            method: "POST",
            headers:{"Content-Type": "application/json",
            },
            body:`{"name":"${name}","UPC":${upc},"quantity":${num}}` 
        })
    }

    const barcodePost = async (img) => {
        const splitArr = img.split(':')
        const json = {
            "data":`${splitArr[1].split(',')[1]}`,
        }
        await fetch(`${HOST}/barcode`, {
            method: "POST",
            headers:{"Content-Type": "application/json",
            },
            body:JSON.stringify(json)})
    }

    return(
        <>
            <Modal buttonText='Add'>
                <form >
                    <label>Item name:
                        <input 
                            type='text'
                            value={name}
                            onChange={(e)=>setName(e.target.value)}/>
                    </label>
                    <label>Item UPC:
                        <input 
                            type="number"
                            value={upc}
                            onChange={(e)=>setUPC(e.target.value)}/>
                    </label>
                    <label>Item count:
                        <input 
                            type="number"
                            value={num}
                            onChange={(e)=>setNum(e.target.value)}/>
                    </label>
                   <input type='button' onClick={onSubmit} value="Submit"/> 
                </form>

                <Cam frameMethod={barcodePost} buttonText='Scan Barcode'/>
            </Modal>
        </>        
    )
            
    
}