import { useEffect, useState } from "react";
import Modal from './Modal'

export default Add

function Add(){
    const [name, setName] = useState(undefined)
    const [upc, setUPC] = useState(undefined)
    const [num, setNum] = useState(undefined)

    const onSubmit = async (e) =>{
        //alert(`name: ${name}\nUPC: ${upc}\nnum: ${num}`)
        const response = await fetch('add', {
            method: "POST",
            headers:{"Content-Type": "application/json",
            },
            body: JSON.stringify({
                "name": {name},
                "upc": {upc},
                "quantity": {num}
            })
        })
        //const response = await fetch('test')
        console.log(`name:${name} upc:${upc} quantity:${num}`)
        alert(response.status)
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
            </Modal>
        </>        
    )
            
    
}