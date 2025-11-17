import React, { useState, useEffect } from 'react';
import {
    AppBar,
    Box,
    Drawer,
    Fab,
    IconButton,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Toolbar,
    Typography,
    useMediaQuery,
    useTheme,
    Paper
} from '@mui/material';
import { DragDropContext, Droppable, Draggable } from '@hello-pangea/dnd';


import { useNavigate } from 'react-router-dom';

import MilestoneList from './MilestoneList';
/* 
mock data
    mock milestones
    mock projects

    join projects to milestones for mock data

    */

const milestones = [
    { id: 101, name: "development" },
    { id: 102, name: "preproduction" },
    { id: 103, name: "production" },
    { id: 104, name: "postproduction" },
    { id: 105, name: "invoiced" },
    { id: 106, name: "completed" }
]

const mockData = [
    {
        // project
        project_id: 0,
        project_short_name: "CedarPeak Product Promo",
        project_title: "Titanium Cookset Product Launch Video",

        // milestone
        milestone_id: 101
    },
    {
        // project
        project_id: 1,
        project_short_name: "E Coffee Campaign",
        project_title: "Emberlane Coffee Autumn Blend Social Media Campaign",

        // milestone
        milestone_id: 102
    },
    {
        // project
        project_id: 2,
        project_short_name: "BlossomWell promo",
        project_title: "BlossomWell Wellness App Promotional Video",

        // milestone
        milestone_id: 103
    },
    {
        // project
        project_id: 3,
        project_short_name: "PulseTrack Launch",
        project_title: "PulseTrack Inventory Management System Industrial Video",

        // milestone
        milestone_id: 101
    },
    {
        // project
        project_id: 4,
        project_short_name: "NovaBloom Candles Ad",
        project_title: "NovaBloom Scented Candles TV Ad",

        // milestone
        milestone_id: 102
    },
    {
        // project
        project_id: 5,
        project_short_name: "IronWeave Safety Video",
        project_title: "IronWeave Safety Equipment Training Video",

        // milestone
        milestone_id: 103
    }


]



const Board = ({ projects }) => {
    const navigate = useNavigate();

    // get milestonesList


    // // use state 
    const [milestoneState, setMilestones] = useState([milestones]);



    // helper functions

    return (
        // Drag Drop Context

        // Droppable Component


        // map milestonesList components with draggable element


        <div>
            <DragDropContext onDragStart={() => console.log('drag started')} onDragEnd={async (results) => {
                console.log('drag ended', results);
            }}>
                <Droppable droppableId="boardDroppable" type="listDrag" direction="horizontal">
                    {(provided) => (
                        <Box className="board"
                            sx={{ display: 'flex', gap: 3 }}
                            ref={provided.innerRef}
                            {...provided.droppableProps}
                        >
                            {milestones.map((milestone, index) => (
                                <Draggable draggableId={milestone.id.toString()} key={milestone.id} index={index}>
                                    {(provided) => (
                                        <Box
                                            ref={provided.innerRef}
                                            {...provided.draggableProps}
                                            {...provided.dragHandleProps}
                                        >
                                            <MilestoneList key={milestone.id} milestone={milestone} projects={mockData.filter(project => project.milestone_id === milestone.id)} navigate={navigate} index={index} />
                                        </Box>
                                    )}
                                </Draggable>
                                // <MilestoneList key={milestone.id} milestone={milestone} projects={mockData.filter(project => project.milestone_id === milestone.id)} navigate={navigate} index={index} />
                            ))
                            }
                            {provided.placeholder}
                        </Box>
                    )}

                </Droppable>

            </DragDropContext>
            {/* <Box sx={{ display: 'flex', gap: 3 }}>
                {milestones.map((milestone) => (
                    <MilestoneList key={milestone.id} milestone={milestone} projects={mockData.filter(project => project.milestone_id === milestone.id)} navigate={navigate} />
                ))
                }
            </Box> */}
        </div>
    )

}

export default Board;