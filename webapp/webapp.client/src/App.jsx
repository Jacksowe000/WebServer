import { useEffect, useState } from 'react';
import './App.css';

export const HOST = 'localhost:7093'

function App() {
    const [inventory, setInventory] = useState();

    useEffect(() => {
        populateInventory();
    }, []);

    const contents = inventory === undefined
        ? <p><em>Loading... FUCK</em></p>
        :<table>
            <thead>
                <tr>
                    <th>
                        Name
                    </th>
                    <th>
                        UPC
                    </th>
                    <th>
                        Quantity
                    </th>
                </tr>
            </thead>
            <tbody>
                {inventory.map(item =>
                    <tr key={item.name}>
                        <td>{item.name}</td>
                        <td>{item.upc}</td>
                        <td>{item.quantity}</td>
                    </tr>
                )}
            </tbody>
        </table> 
    return (
        <div>
            <h1 id="tabelLabel">Inventory</h1>
            {contents}
        </div>
    );
    
    async function populateInventory() {
        console.log('fetching')
        const response = await fetch(`inventory`);
        console.log(response.body)
        const data = await response.json();
        console.log(data)
        setInventory(data);
    }
}

export default App;